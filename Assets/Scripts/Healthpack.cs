using UnityEngine;

public class Healthpack : MonoBehaviour {

    public Player playerScript;
    public CharacterController playerController;

    public float healingAmount = 50f;
    public bool destroyAfterUse = true;

    private AudioSource[] audioSource; //order: footsteps, out of breath, jump, take damage, heal, bg, receivestamina
    private float cHealth;
    private float mHealth;

    void Start() {
        audioSource = playerController.GetComponents<AudioSource>(); //gets the heal sound component from the players gameobject. reason: if the component is on healthpack object, wont play after destroy
    }

    void Update() {
        cHealth = playerScript.currentHealth;
        mHealth = playerScript.maxHealth;
        //Debug.Log("Max health:" + mHealth + "Current health:" + cHealth);
    }

    void OnTriggerEnter(Collider collider){
        if(cHealth != mHealth) {
            if(collider.gameObject.transform.tag == "Player"){

                playerScript.TakeHealing(healingAmount);
                audioSource[4].Play(); //heal sound

                if(destroyAfterUse) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
