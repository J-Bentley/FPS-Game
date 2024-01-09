using UnityEngine;

public class GunSway : MonoBehaviour {

    [SerializeField] private float smoothing;
    [SerializeField] private float swayMultiplier;

    void Update() {
        LookSway();
    }

    void LookSway() {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRot = rotX * rotY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smoothing * Time.deltaTime);
    }
}
