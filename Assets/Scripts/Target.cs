using UnityEngine;
using UnityEngine.UI;

// put on enemies: takes damage, displays healthbar, plays hurt sound, destroy on death, death sound, spawns destroyed version prefab
// can be modified to also work for destructible props -- make healthbar optional, hurt/death sounds can be breaking sounds.

public class Target : MonoBehaviour {
    [SerializeField] GameObject destroyedVersion;
    [SerializeField] bool giveMoneyOnDeath;
    [SerializeField] bool giveMoneyOnDamage;
    [SerializeField] bool destroyPeices;
    [SerializeField] bool playHurtSound;
    [SerializeField] bool playDeathSound;
    [SerializeField] float destroyPeicesTimer;
    [SerializeField] float targetHealth;
    [SerializeField] int minRandomDeathMoney;
    [SerializeField] int maxRandomDeathMoney;
    [SerializeField] int minRandomHurtMoney;
    [SerializeField] int maxRandomHurtMoney;
    [SerializeField] Slider enemyHealthbar = null;
    AudioSource hurtSound;
    AudioSource deathSound;
    bool isDead = false;

    void Start(){
        enemyHealthbar.maxValue = targetHealth;
        enemyHealthbar.value = targetHealth;
        hurtSound = GetComponent<AudioSource>();
    }

    public void TakeTargetDamage (float amount) { 
        targetHealth -= amount;
        enemyHealthbar.value = targetHealth;
        
        if (giveMoneyOnDamage) {
            int randomAmount = Random.Range(minRandomHurtMoney, maxRandomHurtMoney);
            Player.instance.ReceiveMoney(randomAmount);
        }

        if (playHurtSound) {
            hurtSound.pitch = Random.Range(0.9f, 1.1f);
            if (!hurtSound.isPlaying) {
                hurtSound.Play();
            }
        }

        if (targetHealth <= 0f) {
            if(!isDead) {
                Die();
            }
        }
    }

    void Die () {
        isDead = true;
        Destroy(gameObject);
        GameObject destroyedObject = Instantiate(destroyedVersion, transform.position, transform.rotation);
        
        foreach (Transform child in destroyedObject.transform) {
            child.GetComponent<Rigidbody>().AddForce(-child.transform.forward * Gun.bulletForce / 20f, ForceMode.Impulse);
        }

        if (giveMoneyOnDeath) {
            int randomAmount = Random.Range(minRandomDeathMoney, maxRandomDeathMoney);
            Player.instance.ReceiveMoney(randomAmount);
        }

        if (playDeathSound) {
            deathSound = destroyedObject.GetComponent<AudioSource>(); //death sound component is on destroyed version prefab
            deathSound.pitch = Random.Range(0.9f, 1.1f);
            if (!deathSound.isPlaying) {
                deathSound.Play();
            }
        }

        if(destroyPeices) {
            Destroy(destroyedObject, destroyPeicesTimer);
        }
    }
}
