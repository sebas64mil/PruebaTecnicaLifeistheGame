using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private Camera playerCamera;

    private PlayerInputHandler input;
    private Rigidbody rb;
    private float xRotation;

    // ------------------ Initializes required components and configures rigidbody and camera references ------------------
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputHandler>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        data.playerCamera = playerCamera;
        data.cameraTransform = playerCamera.transform;
    }

    // ------------------ Handles physics-based player movement using fixed timestep ------------------
    void FixedUpdate()
    {
        Move();
    }

    // ------------------ Handles camera and player rotation based on look input ------------------
    void Update()
    {
        Look();
    }

    // ------------------ Calculates and applies movement forces relative to the camera orientation ------------------
    private void Move()
    {
        Vector3 forward = data.cameraTransform.forward;
        Vector3 right = data.cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        Vector3 direction =
            forward.normalized * input.MoveInput.y +
            right.normalized * input.MoveInput.x;

        Vector3 targetVelocity = direction * data.moveSpeed;
        Vector3 current = rb.linearVelocity;

        Vector3 velocityChange = targetVelocity - new Vector3(
            current.x,
            0f,
            current.z
        );

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        Vector3 horizontalVelocity = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        if (horizontalVelocity.magnitude > data.maxVelocity)
        {
            horizontalVelocity = horizontalVelocity.normalized * data.maxVelocity;
            rb.linearVelocity = new Vector3(
                horizontalVelocity.x,
                rb.linearVelocity.y,
                horizontalVelocity.z
            );
        }
    }

    // ------------------ Controls camera pitch and player yaw based on mouse or look input ------------------
    private void Look()
    {
        if (GameManager.isPaused)
            return;

        float lookX = input.LookInput.x * data.lookSensitivity;
        float lookY = input.LookInput.y * data.lookSensitivity;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, data.minLookAngle, data.maxLookAngle);

        data.cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);
    }
}
