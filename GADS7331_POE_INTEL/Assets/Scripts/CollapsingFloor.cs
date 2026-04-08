using UnityEngine;

public class CollapsingFloor : MonoBehaviour
{
    public float fallDelay = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Invoke("Collapse", fallDelay);
    }
    private void Collapse() => GetComponent<Rigidbody>().isKinematic = false;
}
