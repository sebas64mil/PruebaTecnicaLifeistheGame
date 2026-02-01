using UnityEngine;

public class OrbitalWeapon : WeaponBase
{
    [SerializeField] private Transform firePoint;
    private ObjectPool projectilePool;

    [Header("Pooling")]
    [SerializeField] private Transform projectileContainer;

    [Header("Particles")]
    [SerializeField] private ParticleSystem muzzleFlash;

    protected override void Awake()
    {
        // ----------------- Initialize weapon ammo and create the projectile object pool ------------------
        base.Awake();

        if (data.projectilePrefab != null)
        {
            projectilePool = new ObjectPool(
                data.projectilePrefab,
                data.poolSize,
                projectileContainer
            );
        }
    }

    public override void Fire()
    {
        // ----------------- Fire an orbital projectile using pooling and fire rate control ------------------
        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo");
            return;
        }

        if (Time.time < lastFireTime + 1f / data.fireRate)
            return;

        muzzleFlash.Play();

        lastFireTime = Time.time;
        currentAmmo--;

        GameObject projectile = projectilePool.Get();
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        OrbitProjectile orbitProj = projectile.GetComponent<OrbitProjectile>();
        if (orbitProj != null)
        {
            orbitProj.Launch(firePoint.forward, data.baseForce);
        }

        PrintWeaponInfo();
    }
}
