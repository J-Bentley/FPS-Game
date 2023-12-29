using UnityEngine;

public class CameraRotate : MonoBehaviour {
    [SerializeField] private Transform target;
    void Update() {
        transform.RotateAround(target.position, target.right, Time.deltaTime * 10f);
    }
}
