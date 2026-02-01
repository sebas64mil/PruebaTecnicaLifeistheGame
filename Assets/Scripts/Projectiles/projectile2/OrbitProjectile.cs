using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(OrbitProjectileStats))]
public class OrbitProjectile : MonoBehaviour, IPoolable
{
    private OrbitProjectileStats stats;

    private ObjectPool pool;
    private float timer;

    private Vector3 direction;
    private float launchSpeed;
    private bool isLaunched = false;

    private readonly List<IOrbitable> orbitingObjects = new();

    // ------------------ Initializes the OrbitProjectileStats reference ------------------
    private void Awake()
    {
        stats = GetComponent<OrbitProjectileStats>();
        Debug.Log(stats.orbitRadius + "radio de la esfera projectile");
    }

    // ------------------ Sets the pool that manages this projectile ------------------
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    // ------------------ Launches the projectile in a direction with a given speed ------------------
    public void Launch(Vector3 dir, float speed)
    {
        direction = dir.normalized;
        launchSpeed = speed;
        isLaunched = true;
        timer = 0f;
        orbitingObjects.Clear();
    }

    // ------------------ Updates projectile movement, detects orbitables, and checks lifetime ------------------
    private void Update()
    {
        timer += Time.deltaTime;

        if (isLaunched)
            transform.position += direction * launchSpeed * Time.deltaTime;

        DetectOrbitables();

        if (timer >= stats.lifeTime)
            ReturnToPool();
    }

    // ------------------ Detects objects that implement IOrbitable within orbit radius ------------------
    private void DetectOrbitables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.orbitRadius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("moveObject")) continue;
            if (!hit.TryGetComponent<IOrbitable>(out var orbitable)) continue;
            if (orbitingObjects.Contains(orbitable)) continue;

            orbitingObjects.Add(orbitable);
            orbitable.StartOrbit(transform);
        }
    }

    // ------------------ Returns projectile to pool when hitting non-orbitable objects ------------------
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IOrbitable>(out _))
        {
            ReturnToPool();
        }
    }

    // ------------------ Stops orbiting objects and returns projectile to pool ------------------
    private void ReturnToPool()
    {
        foreach (var orbitable in orbitingObjects)
            orbitable.StopOrbit();

        orbitingObjects.Clear();
        isLaunched = false;
        direction = Vector3.zero;
        launchSpeed = 0f;

        if (pool != null)
            pool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    // ------------------ Draws orbit radius gizmo in editor ------------------
    private void OnDrawGizmos()
    {
        if (stats == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stats.orbitRadius);
    }
#endif
}
