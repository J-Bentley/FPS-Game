using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour {
    
    [SerializeField] private Transform playerBody;
    public static float lookSensitivity = 500f;
    private float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        //hides cursor
    }

    void Update() {
        Debug.Log(lookSensitivity);
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //stops player from looking behind them

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        //y axis moves just the camera, x axis rotates the player as well

    }
}
