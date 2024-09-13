using UnityEngine;

public class CameraTilt : MonoBehaviour {
    [SerializeField] Transform playerTransform;
    [SerializeField]  float tiltAngle = 10f;    
    [SerializeField]  float tiltSpeed = 5f;      
    [SerializeField]  float returnSpeed = 2f;    
    Vector3 previousPosition;

    void Start() {
        previousPosition = playerTransform.position;
    }

    void Update() {
        // Manually calculate movement direction in world space
        Vector3 currentPosition = playerTransform.position;
        Vector3 worldMovementDirection = (currentPosition - previousPosition).normalized;
        previousPosition = currentPosition;

        // Convert world movement direction to local space relative to the player's facing direction
        Vector3 localMovementDirection = playerTransform.InverseTransformDirection(worldMovementDirection);

        // Check if the player is moving
        bool isMoving = Input.GetAxis("Horizontal") > 0  || Input.GetAxis("Horizontal") < 0;

        Quaternion targetRotation;

        if (isMoving) {
            // Calculate tilt angles based on the local movement direction
            //float targetTiltX = localMovementDirection.z * tiltAngle; // Forward/Backward tilt (Z-axis)
            float targetTiltZ = -localMovementDirection.x * tiltAngle;  // Side-to-side tilt (X-axis)

            // Create the target rotation based on the tilt angles
            targetRotation = Quaternion.Euler(0, 0, targetTiltZ);
        }
        else
        {
            // If the player is not moving, return the camera to its original rotation
            targetRotation = Quaternion.identity;
        }

        // Smoothly rotate the camera towards the target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * (isMoving ? tiltSpeed : returnSpeed));
    }
}