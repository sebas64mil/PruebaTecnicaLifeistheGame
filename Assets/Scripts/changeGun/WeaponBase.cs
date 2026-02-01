using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData data;

    protected int currentAmmo;
    protected float lastFireTime;

    protected virtual void Awake()
    {
        // ----------------- Initialize current ammo using weapon data ------------------
        currentAmmo = data.maxAmmo;
    }

    public void GetAmmoInfo(out int current, out int max)
    {
        // ----------------- Return current and maximum ammo values ------------------
        current = currentAmmo;
        max = data.maxAmmo;
    }

    public abstract void Fire();

    protected void PrintWeaponInfo()
    {
        // ----------------- Print weapon debug information to the console ------------------
        Debug.Log(
            $"Weapon: {data.weaponName} | Ammo: {currentAmmo}/{data.maxAmmo} | Force: {data.baseForce}"
        );
    }
}
