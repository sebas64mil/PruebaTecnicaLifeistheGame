using UnityEngine;

public class ParabolicWeapon : WeaponBase
{
    [Header("Weapon References")]
    [SerializeField] private Transform firePoint;

    [Header("Pooling")]
    [SerializeField] private Transform projectileContainer;

    [Header("Trajectory Preview")]
    [SerializeField] private ParabolicTrajectoryPreview trajectoryPreview;

    [Header("Particles")]
    [SerializeField] private ParticleSystem muzzleFlash;

    private ObjectPool projectilePool;

    protected override void Awake()
    {
        // ----------------- Initialize weapon ammo and projectile object pool ------------------
        base.Awake();

        if (data.projectilePrefab != null && projectileContainer != null)
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
        // ----------------- Fire a parabolic projectile using pooling and fire rate control ------------------
        if (currentAmmo <= 0)
            return;

        if (Time.time < lastFireTime + 1f / data.fireRate)
            return;

        if (projectilePool == null || firePoint == null)
            return;

        lastFireTime = Time.time;
        currentAmmo--;

        muzzleFlash.Play();

        GameObject projectile = projectilePool.Get();
        projectile.transform.SetPositionAndRotation(
            firePoint.position,
            firePoint.rotation
        );

        if (projectile.TryGetComponent(out ParabolicProjectile proj))
            proj.Launch(firePoint.forward, data.baseForce);

        PrintWeaponInfo();
    }

    public void ShowTrajectory()
    {
        // ----------------- Display the parabolic trajectory preview ------------------
        if (trajectoryPreview == null || firePoint == null)
            return;

        trajectoryPreview.Show();
    }

    public void HideTrajectory()
    {
        // ----------------- Hide the parabolic trajectory preview ------------------
        if (trajectoryPreview == null)
            return;

        trajectoryPreview.Hide();
    }
}
