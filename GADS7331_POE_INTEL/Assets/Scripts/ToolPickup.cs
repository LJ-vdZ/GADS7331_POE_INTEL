using UnityEngine;

public class ToolPickup : MonoBehaviour
{
    //need to change this one. show player holding tool with text showing the number of tools collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { Debug.Log("Tool collected"); Destroy(gameObject); }
    }
}
