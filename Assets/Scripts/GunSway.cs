using UnityEngine;

public class GunSway : MonoBehaviour {

    [SerializeField] private float smoothing;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySpeed = 2f;
    private Vector3 originalPosition;

    void Update() {
        LookSway();
        //IdleSway();
    }
    void Start() {
        originalPosition = transform.localPosition;
    }

    void LookSway() {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRot = rotX * rotY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smoothing * Time.deltaTime);
    }

    void IdleSway() {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && !Input.GetButton("Fire2")) {
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            float swayY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;
            Vector3 sway = new Vector3(swayX, swayY, 0);
            transform.localPosition = originalPosition + sway;
        }
        else {
            transform.localPosition = originalPosition;
        }
    }
}
