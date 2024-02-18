using UnityEngine;

public class Healthpack : MonoBehaviour {

    private AudioSource[] audioSource; 
    [SerializeField] private float healingAmount = 50f;
    [SerializeField] private bool destroyAfterUse = true;
    [SerializeField] private bool playSound = true;

    void Start() {
        audioSource = SpawnPlayer.playerInstance.GetComponents<AudioSource>();
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.transform.tag == "Player") {
            if (SpawnPlayer.playerInstance.GetComponent<Player>().currentHealth != SpawnPlayer.playerInstance.GetComponent<Player>().maxHealth) {
                SpawnPlayer.playerInstance.GetComponent<Player>().ReceiveHealing(healingAmount);
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
