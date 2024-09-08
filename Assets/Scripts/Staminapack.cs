using UnityEngine;

public class Staminapack : MonoBehaviour {
    AudioSource audioSource;
    [SerializeField] float staminaAmount = 5f;
    [SerializeField] bool destroyAfterUse = true;
    [SerializeField] bool playSound = true;


    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            Player.instance.ReceiveStamina(staminaAmount);
            if (playSound) {
                audioSource.Play();
            }
            if (destroyAfterUse) {
                Destroy(gameObject);
            }
        }
    }
}
