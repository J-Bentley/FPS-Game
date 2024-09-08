using UnityEngine;

public class IdleSway : MonoBehaviour {
    public float swaySpeed;
    public float swayAmount;

    void Update() {// TODO: do this smartererer
        if (Gun.gunEquipped) {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { 
                float walkSwayX = Mathf.Sin(Time.time * swaySpeed * 2f) * swayAmount;
                float walkSwayY = Mathf.Cos(Time.time * swaySpeed * 8f) * swayAmount * 3f;
                Vector3 walkSway = new Vector3(walkSwayX, walkSwayY, 0f);
                Gun.gunObject.transform.localPosition = Vector3.Lerp(Gun.gunObject.transform.localPosition, walkSway, Time.deltaTime * 6f);
                if (Input.GetKey("left shift") && Player.currentStamina > 0) { 
                    float sprintSwayX = Mathf.Sin(Time.time * swaySpeed * 4f) * swayAmount;
                    float sprintSwayY = Mathf.Cos(Time.time * swaySpeed * 12f) * swayAmount * 3f; // any higher and may clip with headbob script
                    Vector3 sprintSway = new Vector3(sprintSwayX, sprintSwayY, 0f);
                    Gun.gunObject.transform.localPosition = Vector3.Lerp(Gun.gunObject.transform.localPosition, sprintSway, Time.deltaTime * 6f);
                }
            } else { 
                float idleSwayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
                float idleSwayY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;
                Vector3 idleSway = new Vector3(idleSwayX, idleSwayY, 0f);
                Gun.gunObject.transform.localPosition = Vector3.Lerp(Gun.gunObject.transform.localPosition, idleSway, Time.deltaTime * 6f);
            }
        }
    }
}
