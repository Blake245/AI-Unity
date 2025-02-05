using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    public string tagName;
    public float maxDistance;
    public float maxAngle;

    public abstract GameObject[] GetGameObjects();

    public virtual bool GetOpenDirection(ref Vector3 openDirection) {  return false; }
}
