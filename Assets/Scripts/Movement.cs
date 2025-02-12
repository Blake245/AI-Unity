using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public MovementData data;

    public virtual Vector3 Velocity { get; set; }
    public virtual Vector3 Acceleration { get; set; }
    public virtual Vector3 Direction { get { return Velocity.normalized; } }

    public virtual Vector3 Destination { get; set; } = Vector3.zero;

    public abstract void ApplyForce(Vector3 force);
    public abstract void MoveTowards(Vector3 position);

    public virtual void Stop() { }
    public virtual void Resume() { }
}
