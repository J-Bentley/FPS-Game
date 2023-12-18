using UnityEngine;

public class MouseLook : MonoBehaviour {
    
    public Transform playerBody;
    public float mouseSensitivity = 500f;
    float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        //hides cursor
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //stops player from looking behind them

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        //y axis moves just the camera, x axis rotates the player as well

    }
}
