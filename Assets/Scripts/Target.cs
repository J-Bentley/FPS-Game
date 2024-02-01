using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private GameObject staminapack;
    [SerializeField] private bool spawnStaminapackOnDeath = true;
    [SerializeField] private bool giveMoneyOnDeath = true;
    [SerializeField] private bool destroyPeices = false;
    [SerializeField] private bool playHurtSound = false;
    [SerializeField] private bool playDeathSound = false;
    [SerializeField] private float destroyPeicesTimer = 10;
    [SerializeField] private float targetHealth = 50f;
    [SerializeField] private int minRandomMoney = 10;
    [SerializeField] private int maxRandomMoney = 20;
    [SerializeField] private Slider enemyHealthbar = null;
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
            hurtSound.pitch = Random.Range(0.9f, 1.1f);
            hurtSound.Play();
        }

        if (targetHealth <= 0f) {
            Die();
        }
    }

    void Die () {
        SpawnEnemies.enemiesKilledThisRound++;
        SpawnEnemies.totalEnemiesKilled++;

        Destroy(gameObject);
        GameObject destroyedObject = Instantiate(destroyedVersion, transform.position, transform.rotation);

        foreach (Transform child in destroyedObject.transform) {
            child.GetComponent<Rigidbody>().AddForce(-child.transform.forward * Gun.bulletForce / 4f, ForceMode.Impulse);
        }
        
        if (spawnStaminapackOnDeath) {
            Instantiate(staminapack, transform.position, transform.rotation);
        }

        if (giveMoneyOnDeath) {
            int randomAmount = Random.Range(minRandomMoney, maxRandomMoney);
            playerScript.ReceiveMoney(randomAmount);
        }

        if (playDeathSound) {
            deathSound = destroyedObject.GetComponent<AudioSource>(); 
            deathSound.pitch = Random.Range(0.9f, 1.1f);
            deathSound.Play();
        }

        if(destroyPeices) {
            Destroy(destroyedObject, destroyPeicesTimer);
        }
    }
}
