using UnityEngine;

public class Staminapack : MonoBehaviour {
    AudioSource[] audioSource;
    [SerializeField] float staminaAmount = 5f;
    [SerializeField] bool destroyAfterUse = true;
    [SerializeField] bool playSound = true;


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
