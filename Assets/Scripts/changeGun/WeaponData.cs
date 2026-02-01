using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Weapon")]
public class WeaponData : ScriptableObject
{
    [Tooltip("Weapon name displayed in the Inspector")]
    public string weaponName;

    [Tooltip("Projectile prefab fired by this weapon")]
    public GameObject projectilePrefab;

    [Header("Weapon")]
    [Tooltip("Fire rate (shots per second)")]
    public float fireRate;

    [Tooltip("Maximum ammunition capacity")]
    public int maxAmmo;

    [Tooltip("Initial force applied to the projectile")]
    public float baseForce;

    [Header("Projectile Pool")]
    [Tooltip("Initial size of the projectile object pool")]
    public int poolSize = 10;
}
