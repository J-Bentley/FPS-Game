using UnityEngine;

public class CameraRotate : MonoBehaviour {
    [SerializeField] private Transform subject;
    [SerializeField] private float speed = 10f;
    void Update() {
        transform.RotateAround(subject.position, subject.right, Time.deltaTime * speed);
    }
}
