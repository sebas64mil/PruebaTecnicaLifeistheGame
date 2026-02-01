using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool InteractPressed { get; private set; }
    public bool FirePressed { get; private set; }
    public bool DropPressed { get; private set; }

    // ------------------ Reads and stores player movement input ------------------
    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    // ------------------ Reads and stores player look input ------------------
    public void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }

    // ------------------ Handles interact input press state ------------------
    public void OnInteract(InputValue value)
    {
        InteractPressed = value.isPressed;
    }

    // ------------------ Handles drop input press state ------------------
    public void OnDrop(InputValue value)
    {
        DropPressed = value.isPressed;
    }

    // ------------------ Handles fire input while respecting pause state ------------------
    public void OnFire(InputValue value)
    {
        if (GameManager.isPaused)
            return;

        Debug.Log("OnFire llamado");
        FirePressed = value.isPressed;
    }

    // ------------------ Resets interact input after it has been processed ------------------
    public void ConsumeInteract()
    {
        InteractPressed = false;
    }

    // ------------------ Resets fire input after it has been processed ------------------
    public void ConsumeFire()
    {
        FirePressed = false;
    }

    // ------------------ Resets drop input after it has been processed ------------------
    public void ConsumeDrop()
    {
        DropPressed = false;
    }
}
