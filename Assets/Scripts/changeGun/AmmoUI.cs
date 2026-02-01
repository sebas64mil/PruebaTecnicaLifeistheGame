using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private PlayerWeaponPickup playerWeapon;

    private WeaponBase currentWeapon;

    void Awake()
    {
        // ----------------- Initialize ammo UI with empty values ------------------
        SetEmpty();
    }

    void Update()
    {
        // ----------------- Update ammo display based on the currently equipped weapon ------------------
        currentWeapon = playerWeapon.CurrentWeapon;

        if (currentWeapon != null)
            UpdateAmmo();
        else
            SetEmpty();
    }

    void UpdateAmmo()
    {
        // ----------------- Retrieve ammo data from the weapon and update the UI text ------------------
        currentWeapon.GetAmmoInfo(out int current, out int max);
        ammoText.text = $"{current}/{max}";
    }

    void SetEmpty()
    {
        // ----------------- Display empty ammo values when no weapon is equipped ------------------
        ammoText.text = "0 / 0";
    }
}
