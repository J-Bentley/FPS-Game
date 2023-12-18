//apply this script to objects you with to deal damage to and destroy.
//set health, reference a destroyed version of the model, can destroy peices after timer, can play hurt/death sounds, can spawn healthpack
//put death sound component on destroyedversion prefab and hurt sound component on target object
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    public float targetHealth = 50f;
    public GameObject destroyedVersion;
    public bool destroyPeices = false;
    public float destroyPeicesTimer = 10;
    public bool playHurtSound = false;
    public bool playDeathSound = false;
    private AudioSource hurtSound;
    private AudioSource deathSound;
    public Slider enemyHealthbar = null;
    public GameObject staminapack;
    public bool spawnStaminapackOnDeath = true;

    void Start(){
        enemyHealthbar.maxValue = targetHealth;
        enemyHealthbar.value = targetHealth;
        hurtSound = GetComponent<AudioSource>(); // gets the hurt sound component on the target
    }

    public void TakeTargetDamage (float amount) { 
        // damage is passed as amount when invoked
        targetHealth -= amount;
        enemyHealthbar.value = targetHealth;
        //Debug.Log(transform.name + " took " + amount + " damage! It has " + targetHealth + " health left.");
        if (playHurtSound) {
            hurtSound.Play();
        }

        if (targetHealth <= 0f) {
            Die();
        }
    }

    void Die () {
        Instantiate(staminapack, transform.position, transform.rotation);
        Destroy(gameObject); 
        GameObject destroyedGO = Instantiate(destroyedVersion, transform.position, transform.rotation);
        deathSound = destroyedGO.GetComponent<AudioSource>(); // gets the death sound component on the destroyed version
        if (playDeathSound) {
            deathSound.Play();
        }

        if(destroyPeices) {
            Destroy(destroyedGO, destroyPeicesTimer); //destroys destroyed version
        }
    }
}
