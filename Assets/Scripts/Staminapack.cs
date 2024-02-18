using UnityEngine;

public class Staminapack : MonoBehaviour {
    private AudioSource[] audioSource;
    [SerializeField] private float staminaAmount = 5f;
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private bool playSound = true;


    void Start() {
        audioSource = SpawnPlayer.playerInstance.GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            SpawnPlayer.playerInstance.GetComponent<Player>().ReceiveStamina(staminaAmount);
            if (playSound) {
                audioSource[6].Play(); //stamina pack sound
            }
            if (destroyAfterUse) {
                Destroy(gameObject);
            }
        }
    }
}
