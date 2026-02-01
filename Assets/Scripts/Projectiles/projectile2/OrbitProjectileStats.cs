using UnityEngine;

public class OrbitProjectileStats : MonoBehaviour
{
    [Header("Lifetime")]
    [Tooltip("Time in seconds before the projectile is returned to the pool")]
    public float lifeTime = 5f;

    [Header("Orbit Field")]
    [Tooltip("Radius in which objects start orbiting the projectile")]
    public float orbitRadius = 5f;
}
