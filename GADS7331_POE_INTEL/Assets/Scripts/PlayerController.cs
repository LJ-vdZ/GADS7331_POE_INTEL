using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 6f;

    public float lookSpeed = 2f;

    public float gravity = 20f;

    private CharacterController characterController;

    private Vector3 moveDir = Vector3.zero;

    private float xRotation = 0f;

    private void Awake() => characterController = GetComponent<CharacterController>();

    private void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;

        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Movement
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        moveDir = (forward * v + right * h) * walkSpeed;

        if (!characterController.isGrounded) moveDir.y -= gravity * Time.deltaTime;

        characterController.Move(moveDir * Time.deltaTime);
    }
}
