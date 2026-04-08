using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    public DoorController targetDoor;
    private int pressedCount = 0;

    public void PlatePressed() { pressedCount++; if (pressedCount >= 2) targetDoor.Unlock("Door3"); }
    public void PlateReleased() { pressedCount = Mathf.Max(0, pressedCount - 1); }

    public void HoldSecondPlate() { pressedCount = 2; targetDoor.Unlock("Door3"); } // Echo remote help
}
