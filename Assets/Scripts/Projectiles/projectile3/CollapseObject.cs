using UnityEngine;

public class CollapseObject : MonoBehaviour, ICollapse
{
    private Rigidbody rb;
    private Collider col;
    private Transform center;

    private bool collapsing;
    private bool held;

    private Vector3 holdOffset;

    public Vector3 Position => transform.position;
    public bool IsHeld => held;

    // ----------------- Initialize Rigidbody and Collider references ------------------
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // ----------------- Start being affected by a CollapseProjectile ------------------
    public void StartCollapse(Transform center)
    {
        this.center = center;
        collapsing = true;
        held = false;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;

        col.isTrigger = true;
    }

    // ----------------- Hold object near the collapse center ------------------
    public void Hold()
    {
        if (center == null) return;

        held = true;
        rb.linearVelocity = Vector3.zero;

        holdOffset = transform.position - center.position;
    }

    // ----------------- Stop being affected by CollapseProjectile ------------------
    public void StopCollapse()
    {
        collapsing = false;
        held = false;
        center = null;

        rb.useGravity = true;
        col.isTrigger = false;
    }

    // ----------------- Apply external force to the object ------------------
    public void ApplyForce(Vector3 force)
    {
        if (held) return;

        rb.linearVelocity += force;
    }

    // ----------------- Update object position according to collapse logic ------------------
    private void FixedUpdate()
    {
        if (!collapsing || center == null) return;

        if (held)
        {
            rb.MovePosition(center.position + holdOffset);
            return;
        }

        rb.MovePosition(transform.position + rb.linearVelocity * Time.fixedDeltaTime);
    }
}
