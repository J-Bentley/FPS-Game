using UnityEngine;

public class IdleSway : MonoBehaviour {
    public Gun gunScript;
    public Player playerScript;
    public float swaySpeed;
    public float swayAmount;

    void Update() {// TODO: do this smartererer
        if (gunScript.gunEquipped) {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { 
                float walkSwayX = Mathf.Sin(Time.time * swaySpeed * 2f) * swayAmount;
                float walkSwayY = Mathf.Cos(Time.time * swaySpeed * 8f) * swayAmount * 3f;
                Vector3 walkSway = new Vector3(walkSwayX, walkSwayY, 0f);
                gunScript.gunObject.transform.localPosition = Vector3.Lerp(gunScript.gunObject.transform.localPosition, walkSway, Time.deltaTime * 6f);
                if (Input.GetKey("left shift") && playerScript.currentStamina > 0) { 
                    float sprintSwayX = Mathf.Sin(Time.time * swaySpeed * 4f) * swayAmount;
                    float sprintSwayY = Mathf.Cos(Time.time * swaySpeed * 12f) * swayAmount * 3f; // any higher than 3 and cam will clip in gun when ads and sprinting
                    Vector3 sprintSway = new Vector3(sprintSwayX, sprintSwayY, 0f);
                    gunScript.gunObject.transform.localPosition = Vector3.Lerp(gunScript.gunObject.transform.localPosition, sprintSway, Time.deltaTime * 6f);
                }
            } else { 
                float idleSwayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
                float idleSwayY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;
                Vector3 idleSway = new Vector3(idleSwayX, idleSwayY, 0f);
                gunScript.gunObject.transform.localPosition = Vector3.Lerp(gunScript.gunObject.transform.localPosition, idleSway, Time.deltaTime * 6f);
            }
        }
    }
}
