using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    private Vector3 target;

    void Start() {
        target = pointA.position;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == pointB.position) {
            target = pointB.position;
        }
        else if (transform.position == pointA.position) {
            target = pointA.position;
        }
    }
}