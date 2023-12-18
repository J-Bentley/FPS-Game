// test :DDD
using UnityEngine;

public class Billboard : MonoBehaviour {

    public Camera cam;

    void Start() {
        cam = Camera.main;
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
