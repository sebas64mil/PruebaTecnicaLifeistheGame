using UnityEngine;

public class CollapseProjectileStats : MonoBehaviour
{
    [Header("Lifetime")]
    [Tooltip("Time in seconds before the projectile is returned to the pool")]
    public float lifeTime = 5f;

    [Header("Collapse Field")]
    [Tooltip("Radius in which objects are affected by the collapse effect")]
    public float collapseRadius = 5f;

    [Tooltip("Force applied to attract objects towards the projectile")]
    public float attractionForce = 8f;

    [Tooltip("Minimum distance before objects stop being attracted and are held")]
    public float minDistance = 1.5f;

    [Tooltip("Force applied to repel objects after attraction phase ends")]
    public float repelForce = 12f;

    [Tooltip("Duration in seconds of the attraction phase before repelling")]
    public float attractDuration = 2f;
}
