using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ParabolicProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody rb;
    private float timer;
    private ObjectPool pool;

    // ------------------ Initializes Rigidbody reference ------------------
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ------------------ Resets projectile state when enabled ------------------
    private void OnEnable()
    {
        timer = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // ------------------ Sets the pool that manages this projectile ------------------
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    // ------------------ Launches the projectile in a given direction with specified force ------------------
    public void Launch(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    // ------------------ Updates the lifetime timer and returns projectile to pool if expired ------------------
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    // ------------------ Returns the projectile to its pool or disables it if no pool exists ------------------
    private void ReturnToPool()
    {
        if (pool != null)
            pool.Return(gameObject);
        else
            gameObject.SetActive(false);
    }
}
