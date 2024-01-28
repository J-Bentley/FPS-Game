using UnityEngine;

public class Recoil : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] private Transform cam;
    [SerializeField] private float recoilX, recoilY, recoilZ, kickBackZ, snappiness, returnAmount;
    private Vector3 currentRotation, targetRotation, targetPosition, currentPosition, originalPositon;

    void Start() {
        originalPositon = transform.localPosition;
    }

    void Update() {
        if (gunScript.gunEquipped) {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snappiness);
            transform.localRotation = Quaternion.Euler(currentRotation);
            cam.localRotation = Quaternion.Euler(currentRotation);
            Down();
        }
    }

    public void Up() {
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        Down();
    }

    void Down() {
        targetPosition = Vector3.Lerp(targetPosition, originalPositon, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snappiness);
        transform.localPosition = currentPosition;
    }
}