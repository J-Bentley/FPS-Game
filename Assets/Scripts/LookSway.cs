using UnityEngine;

public class LookSway : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float swaySmoothing;

    void Update() {
        if (gunScript.gunEquipped) {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRot = rotX * rotY;
            gunScript.gunObject.transform.localRotation = Quaternion.Slerp(gunScript.gunObject.transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
        }
    }
}
