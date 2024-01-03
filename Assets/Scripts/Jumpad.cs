using UnityEngine;

public class Jumpad : MonoBehaviour {
    public Player playerScript;
    private AudioSource audioSource;
    [SerializeField] private float jumpadHeight;
    [SerializeField] private bool playSound = true;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.Jumpad(jumpadHeight);
            if (playSound) {
                audioSource.Play();
            }
        }
    }
}
