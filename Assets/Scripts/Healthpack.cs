using UnityEngine;

public class Healthpack : MonoBehaviour {

    public Player playerScript;
    private AudioSource[] audioSource; 
    [SerializeField] private CharacterController playerController;
    [SerializeField] private float healingAmount = 50f;
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private bool playSound = true;

    void Start() {
        audioSource = playerController.GetComponents<AudioSource>();
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.transform.tag == "Player") {
            if (playerScript.currentHealth != playerScript.maxHealth) {
                playerScript.ReceiveHealing(healingAmount);
                if (playSound) {
                    audioSource[4].Play(); //heal sound
                }
                if (destroyAfterUse) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
