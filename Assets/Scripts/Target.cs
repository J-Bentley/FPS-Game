using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    [SerializeField] private float targetHealth = 50f;
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private bool destroyPeices = false;
    [SerializeField] private float destroyPeicesTimer = 10;
    [SerializeField] private bool playHurtSound = false;
    [SerializeField] private bool playDeathSound = false;
    [SerializeField] private Slider enemyHealthbar = null;
    [SerializeField] private GameObject staminapack;
    [SerializeField] private GameObject coin;
    [SerializeField] private bool spawnStaminapackOnDeath = true;
    [SerializeField] private bool spawnCoinOnDeath = true;
    private AudioSource hurtSound;
    private AudioSource deathSound;

    void Start(){
        enemyHealthbar.maxValue = targetHealth;
        enemyHealthbar.value = targetHealth;
        hurtSound = GetComponent<AudioSource>();
    }

    public void TakeTargetDamage (float amount) { 
        targetHealth -= amount;
        enemyHealthbar.value = targetHealth;

        if (playHurtSound) {
            hurtSound.Play();
        }

        if (targetHealth <= 0f) {
            Die();
        }
    }

    void Die () {
        SpawnEnemies.enemiesKilled++;
        Destroy(gameObject);
        GameObject destroyedGO = Instantiate(destroyedVersion, transform.position, transform.rotation);

        if (spawnStaminapackOnDeath) {
            Instantiate(staminapack, transform.position, transform.rotation);
        }

        if (spawnCoinOnDeath) {
            Instantiate(coin, transform.position, transform.rotation);
        }

        if (playDeathSound) {
            deathSound = destroyedGO.GetComponent<AudioSource>(); 
            deathSound.Play();
        }

        if(destroyPeices) {
            Destroy(destroyedGO, destroyPeicesTimer);
        }
    }
}
