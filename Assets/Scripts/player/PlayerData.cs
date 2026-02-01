using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float maxVelocity = 8f;

    [Header("Look")]
    public float lookSensitivity = 2f;
    public float minLookAngle = -80f;
    public float maxLookAngle = 80f;

    [Header("References (Runtime)")]
    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public Transform cameraTransform;
}
