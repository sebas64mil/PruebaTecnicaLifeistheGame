using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float weaponZOffset = 0.4f;
    public WeaponBase CurrentWeapon => currentWeapon;

    private PlayerInputHandler input;

    private WeaponBase currentWeapon;
    private WeaponBase weaponInRange;

    void Awake()
    {
        // ----------------- Initialize player input handler reference ------------------
        input = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        // ----------------- Handle weapon interaction and drop input every frame ------------------
        HandleInteract();
        HandleDrop();
    }

    void HandleInteract()
    {
        // ----------------- Handle weapon pickup or weapon swap when interact input is pressed ------------------
        if (!input.InteractPressed)
            return;

        if (currentWeapon == null && weaponInRange != null)
        {
            PickupWeapon(weaponInRange);
        }
        else if (currentWeapon != null && weaponInRange != null)
        {
            WeaponBase newWeapon = weaponInRange;
            DropWeapon();
            PickupWeapon(newWeapon);
        }

        input.ConsumeInteract();
    }

    void HandleDrop()
    {
        // ----------------- Handle weapon drop when drop input is pressed ------------------
        if (!input.DropPressed)
            return;

        if (currentWeapon != null)
        {
            DropWeapon();
        }

        input.ConsumeDrop();
    }

    private void PickupWeapon(WeaponBase newWeapon)
    {
        // ----------------- Attach the selected weapon to the player and disable its physics ------------------
        currentWeapon = newWeapon;

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
            rb.detectCollisions = false;
            rb.interpolation = RigidbodyInterpolation.None;
        }

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        currentWeapon.transform.SetParent(weaponHolder);
        currentWeapon.transform.localPosition = new Vector3(0f, 0f, weaponZOffset);
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    private void DropWeapon()
    {
        // ----------------- Detach the current weapon from the player and re-enable its physics ------------------
        if (currentWeapon == null) return;

        currentWeapon.transform.SetParent(null);

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        }

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        currentWeapon = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ----------------- Detect when a weapon enters the pickup range ------------------
        if (other.CompareTag("Weapon"))
        {
            weaponInRange = other.GetComponent<WeaponBase>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ----------------- Clear the weapon reference when it leaves the pickup range ------------------
        if (other.CompareTag("Weapon"))
        {
            if (weaponInRange != null && other.gameObject == weaponInRange.gameObject)
            {
                weaponInRange = null;
            }
        }
    }
}
