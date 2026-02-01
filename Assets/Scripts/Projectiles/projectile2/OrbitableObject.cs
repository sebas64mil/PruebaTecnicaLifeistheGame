using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OrbitableObject : MonoBehaviour, IOrbitable
{
    [SerializeField] private float orbitSpeed = 120f;
    [SerializeField] private float orbitRadius = 3f;
    [SerializeField] private float heightOffset = 1.2f;
    [SerializeField] private float enterSmoothTime = 0.5f;

    private Rigidbody rb;
    private Transform center;
    private float angle;
    private bool orbiting;

    private Vector3 orbitAxis;
    private Vector3 initialOffset;
    private Vector3 startPosition;
    private float enterTimer;

    // ------------------ Initializes the Rigidbody reference ------------------
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ------------------ Starts orbiting around a given center ------------------
    public void StartOrbit(Transform center)
    {
        this.center = center;
        orbiting = true;

        rb.useGravity = false;

        orbitAxis = Random.onUnitSphere;

        initialOffset = new Vector3(
            Random.Range(-orbitRadius, orbitRadius),
            Random.Range(0.5f, heightOffset),
            Random.Range(-orbitRadius, orbitRadius)
        );

        angle = Random.Range(0f, 360f);

        startPosition = transform.position;
        enterTimer = 0f;
    }

    // ------------------ Stops orbiting and restores gravity ------------------
    public void StopOrbit()
    {
        orbiting = false;
        center = null;
        rb.useGravity = true;
    }

    // ------------------ Updates the orbit position smoothly ------------------
    private void FixedUpdate()
    {
        if (!orbiting || center == null) return;

        angle += orbitSpeed * Time.fixedDeltaTime;

        Quaternion rot = Quaternion.AngleAxis(orbitSpeed * Time.fixedDeltaTime, orbitAxis);
        initialOffset = rot * initialOffset;

        Vector3 orbitPos = center.position + initialOffset;

        if (enterTimer < enterSmoothTime)
        {
            enterTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(enterTimer / enterSmoothTime);
            Vector3 smoothPos = Vector3.Lerp(startPosition, orbitPos, t);
            rb.MovePosition(smoothPos);
        }
        else
        {
            rb.MovePosition(orbitPos);
        }
    }

    // ------------------ Handles collisions with non-orbitable objects ------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<IOrbitable>(out _))
        {
            orbitAxis = -orbitAxis;
            orbitSpeed *= 0.9f;
        }
    }
}
