using UnityEngine;

public class GravityRoomController : MonoBehaviour
{
    public bool gravityOn = true;

    private Vector3 originalGravity;

    private void Awake() => originalGravity = Physics.gravity;

    public void SetGravity(bool on)
    {
        gravityOn = on;
        Physics.gravity = on ? originalGravity : Vector3.zero;
    }
}
