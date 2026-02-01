using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ParabolicTrajectoryPreview : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponData weaponData;

    [Header("Trajectory Settings")]
    [SerializeField] private int steps = 30;
    [SerializeField] private float stepTime = 0.1f;

    private LineRenderer line;
    [SerializeField] private float projectileMass = 4f;

    // ------------------ Initializes LineRenderer and reads projectile mass ------------------
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        if (weaponData != null && weaponData.projectilePrefab != null)
        {
            if (weaponData.projectilePrefab.TryGetComponent(out Rigidbody rb))
                projectileMass = rb.mass;
        }
    }

    // ------------------ Checks each frame if the weapon is held by the player and shows/hides trajectory ------------------
    private void Update()
    {
        if (!IsHeldByPlayer())
        {
            Hide();
            return;
        }

        Show();
    }

    // ------------------ Determines if this object is parented to the player ------------------
    private bool IsHeldByPlayer()
    {
        Transform current = transform;

        while (current != null)
        {
            if (current.CompareTag("Player"))
                return true;

            current = current.parent;
        }

        return false;
    }

    // ------------------ Calculates and displays the parabolic trajectory ------------------
    public void Show()
    {
        if (firePoint == null || weaponData == null)
            return;

        line.enabled = true;
        line.positionCount = steps;

        Vector3 startPos = firePoint.position;
        Vector3 startVelocity =
            firePoint.forward.normalized * (weaponData.baseForce / projectileMass);

        for (int i = 0; i < steps; i++)
        {
            float t = i * stepTime;
            Vector3 point =
                startPos +
                startVelocity * t +
                0.5f * Physics.gravity * t * t;

            line.SetPosition(i, point);
        }
    }

    // ------------------ Hides the trajectory by disabling the LineRenderer ------------------
    public void Hide()
    {
        if (line.enabled)
            line.enabled = false;
    }
}
