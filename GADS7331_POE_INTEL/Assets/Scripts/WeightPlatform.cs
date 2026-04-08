using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    public DoorController targetDoor;
    private float currentWeight = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody) currentWeight += other.attachedRigidbody.mass;

        if (currentWeight > 40f) targetDoor.Unlock("Door4");
    }
}
