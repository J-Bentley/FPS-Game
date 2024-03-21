using UnityEngine;

public class CameraHeadBob : MonoBehaviour {
    [SerializeField] private float swayMultiplier;
    [SerializeField] private float swaySmoothing;


    void Start() {

    }

    void Update() {
        float forward = Input.GetAxis("Vertical") * swayMultiplier;
        float left = Input.GetAxis("Horizontal") * swayMultiplier;

        Quaternion rotForward = Quaternion.AngleAxis(forward, Vector3.right);
        Quaternion rotLeft = Quaternion.AngleAxis(left, Vector3.forward);

        Quaternion targetRot = rotForward * rotLeft;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
    }
}