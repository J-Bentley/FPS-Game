using UnityEngine;

public class Staminapack : MonoBehaviour {
    [SerializeField] private Player playerScript;
    [SerializeField] private CharacterController playerController;
    private AudioSource[] audioSource;
    [SerializeField] private float staminaAmount = 5f;
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private bool playSound = true;


    void Start() {
        audioSource = playerController.GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.ReceiveStamina(staminaAmount);
            if (playSound) {
                audioSource[6].Play(); //stamina pack sound
            }
            if (destroyAfterUse) {
                Destroy(gameObject);
            }
        }
    }
}
