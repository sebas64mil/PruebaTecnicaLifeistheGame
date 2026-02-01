using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(CollapseProjectileStats))]
public class CollapseProjectile : MonoBehaviour, IPoolable
{
    private CollapseProjectileStats stats;

    private ObjectPool pool;
    private float timer;
    private float attractTimer;

    private Vector3 direction;
    private float launchSpeed;
    private bool isLaunched = false;
    private bool isRepelling = false;

    private readonly List<ICollapse> affectedObjects = new();

    // ----------------- Initialize references ------------------
    private void Awake()
    {
        stats = GetComponent<CollapseProjectileStats>();
    }

    // ----------------- Assign the pool to this projectile ------------------
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    // ----------------- Launch the projectile in a direction with a given speed ------------------
    public void Launch(Vector3 dir, float speed)
    {
        direction = dir.normalized;
        launchSpeed = speed;
        isLaunched = true;
        isRepelling = false;

        timer = 0f;
        attractTimer = 0f;
        affectedObjects.Clear();
    }

    // ----------------- Update projectile position, attraction, repulsion, and lifetime ------------------
    private void Update()
    {
        if (!isLaunched) return;

        timer += Time.deltaTime;

        if (!isRepelling)
        {
            transform.position += direction * launchSpeed * Time.deltaTime;
            attractTimer += Time.deltaTime;
            DetectCollapseObjects();
            ApplyAttraction();
        }

        if (!isRepelling && attractTimer >= stats.attractDuration)
        {
            isRepelling = true;
            ApplyRepel();
        }

        if (timer >= stats.lifeTime)
            ReturnToPool();
    }

    // ----------------- Detect objects in range that implement ICollapse ------------------
    private void DetectCollapseObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.collapseRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("moveObject")) continue;
            if (!hit.TryGetComponent<ICollapse>(out var collapseObj)) continue;
            if (affectedObjects.Contains(collapseObj)) continue;

            affectedObjects.Add(collapseObj);
            collapseObj.StartCollapse(transform);
        }
    }

    // ----------------- Apply attraction force to detected objects ------------------
    private void ApplyAttraction()
    {
        foreach (var obj in affectedObjects)
        {
            if (obj == null || obj.IsHeld) continue;

            Vector3 toCenter = transform.position - obj.Position;
            float dist = toCenter.magnitude;

            if (dist <= stats.minDistance)
            {
                obj.Hold();
                continue;
            }

            float t = Mathf.InverseLerp(stats.minDistance, stats.collapseRadius, dist);
            t = Mathf.SmoothStep(0f, 1f, t);

            Vector3 force = toCenter.normalized * stats.attractionForce * t;
            obj.ApplyForce(force);
        }
    }

    // ----------------- Apply repulsion force to affected objects ------------------
    private void ApplyRepel()
    {
        foreach (var obj in affectedObjects)
        {
            if (obj == null) continue;

            Vector3 randomDir = Random.onUnitSphere;

            if (randomDir.y < 0.2f)
                randomDir.y = 0.2f;

            randomDir.Normalize();

            obj.StopCollapse();
            obj.ApplyForce(randomDir * stats.repelForce);
        }

        affectedObjects.Clear();
    }

    // ----------------- Return to pool if hits something non-collapsible ------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<ICollapse>(out _))
            ReturnToPool();
    }

    // ----------------- Reset projectile and return it to the object pool ------------------
    private void ReturnToPool()
    {
        foreach (var obj in affectedObjects)
            obj.StopCollapse();

        affectedObjects.Clear();
        isLaunched = false;
        isRepelling = false;
        direction = Vector3.zero;
        launchSpeed = 0f;

        pool?.Return(gameObject);
    }

#if UNITY_EDITOR
    // ----------------- Draw collapse radius in the editor ------------------
    private void OnDrawGizmos()
    {
        if (stats == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.collapseRadius);
    }
#endif
}
