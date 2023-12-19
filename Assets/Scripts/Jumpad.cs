
using UnityEngine;

public class Jumpad : MonoBehaviour {
    public Player playerScript;
    [SerializeField] private float jumpadHeight;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.Jumpad(jumpadHeight);
            audioSource.Play();
        }
    }
}
