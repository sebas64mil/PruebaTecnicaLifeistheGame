using UnityEngine;

public class PlayerWeaponFire : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;

    private PlayerInputHandler input;

    // ------------------ Gets and caches the PlayerInputHandler reference ------------------
    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    // ------------------ Checks fire input and triggers the current weapon fire action ------------------
    void Update()
    {
        if (!input.FirePressed)
            return;

        IWeapon weapon = weaponHolder.GetComponentInChildren<IWeapon>();
        if (weapon != null)
        {
            weapon.Fire();
        }

        input.ConsumeFire();
    }
}
