using UnityEngine;

public interface ICollapse
{
    Vector3 Position { get; }
    bool IsHeld { get; }

    void StartCollapse(Transform center);
    void StopCollapse();
    void Hold();               
    void ApplyForce(Vector3 f);
}
