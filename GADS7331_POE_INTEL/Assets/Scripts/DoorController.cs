using UnityEngine;

public class DoorController : MonoBehaviour
{
    public string doorID = "Door2";
    public bool isLocked = true;
    public GameObject doorMesh;

    public void Unlock(string id)
    {
        if (id == doorID) { isLocked = false; doorMesh.SetActive(false); Debug.Log(doorID + " unlocked by Echo!"); }
    }
}
