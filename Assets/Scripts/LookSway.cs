using UnityEngine;

public class LookSway : MonoBehaviour {
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float swaySmoothing;

    void Update() {
        if (Gun.gunEquipped) {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRot = rotX * rotY;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
        }
    }
}
