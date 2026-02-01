using UnityEngine;

public class CollapseWeapon : WeaponBase
{
    [SerializeField] private Transform firePoint;
    private ObjectPool projectilePool;

    [Header("Pooling")]
    [SerializeField] private Transform projectileContainer;

    [Header("Particles")]
    [SerializeField] private ParticleSystem muzzleFlash;

    protected override void Awake()
    {
        // ----------------- Initialize weapon ammo and create the collapse projectile object pool ------------------
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
        // ----------------- Fire a collapse projectile using pooling and fire rate control ------------------
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

        CollapseProjectile collapseProj = projectile.GetComponent<CollapseProjectile>();
        if (collapseProj != null)
        {
            collapseProj.Launch(firePoint.forward, data.baseForce);
        }

        PrintWeaponInfo();
    }
}
