// rotates gun on all axis based on mouse input, clamps up/down/left/right when aiming but will still roll barrel while aiming
using UnityEngine;

public class LookSway : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float swaySmoothing;

    void Update() {
        if (gunScript.gunEquipped) {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            float mouseZ = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            
            if (Input.GetButton("Fire2")) {
                mouseY = Mathf.Clamp(mouseY, -1f, 1f);
                mouseX = Mathf.Clamp(mouseX, -1f, 1f);
            }

            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion rotZ = Quaternion.AngleAxis(mouseZ, Vector3.back);
            
            Quaternion targetRot = rotX * rotY * rotZ;
            gunScript.gunObject.transform.localRotation = Quaternion.Slerp(gunScript.gunObject.transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
        }
    }
}
