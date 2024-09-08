using UnityEngine;

public class Healthpack : MonoBehaviour {

    AudioSource audioSource; 
    [SerializeField] float healingAmount = 50f;
    [SerializeField] bool destroyAfterUse = true;
    [SerializeField] bool playSound = true;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.transform.tag == "Player") {
            if (Player.instance.currentHealth != Player.instance.maxHealth) {
                Player.instance.ReceiveHealing(healingAmount);
                if (playSound) {
                    audioSource.Play();
                }
                if (destroyAfterUse) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
