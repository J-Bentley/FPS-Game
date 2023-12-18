
using UnityEngine;

public class Jumpad : MonoBehaviour {
    public Player playerScript;
    [SerializeField] private float jumpadHeight;

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.Jumpad(jumpadHeight);
        }
    }
}
