using UnityEngine;

public class Staminapack : MonoBehaviour {
    public Player playerScript;
    public CharacterController playerController;
    private AudioSource[] audioSource; //order: footsteps, out of breath, jump, take damage, heal, bg, receivestamina
    public float staminaAmount = 5f;
    public bool destroyAfterUse = true;

    void Start() {
        audioSource = playerController.GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.ReceiveStamina(staminaAmount);
            audioSource[6].Play();
            if (destroyAfterUse) {
                Destroy(gameObject);
            }
        }
    }
}
