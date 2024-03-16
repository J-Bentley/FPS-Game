using UnityEngine;

public class CameraHeadBob : MonoBehaviour {
    [SerializeField] private Vector3 restPosition;
    [SerializeField] private float bobAmount;
    [SerializeField] private float bobSpeed;
    [SerializeField] private float sprintBobAmount;
    [SerializeField] private float sprintBobSpeed;
    [SerializeField] private Player playerScript;
    private float originalBobAmount;
    private float originalBobSpeed;
    private float timer = Mathf.PI / 2;

    void Start() {
        originalBobAmount = bobAmount;
        originalBobSpeed = bobSpeed;
    }

    void FixedUpdate() {
        if (Input.GetKey("left shift") && playerScript.currentStamina > 0) {
            bobAmount = sprintBobAmount;
            bobSpeed = sprintBobSpeed;
        } else {
            bobAmount = originalBobAmount;
            bobSpeed = originalBobSpeed;
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && playerScript.isGrounded) {
            timer += bobSpeed * Time.deltaTime;
            Vector3 newPosition = new Vector3(Mathf.Cos(timer) * bobAmount, restPosition.y + Mathf.Abs(Mathf.Sin(timer) * bobAmount), restPosition.z);
            transform.localPosition = newPosition;
        } else {
            timer = Mathf.PI / 2;
        }

        if (timer > Mathf.PI * 2) {
            timer = 0;    
        }
    }
}