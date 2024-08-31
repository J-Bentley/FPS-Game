using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //[SerializeField] Player playerScript;
    AudioSource[] hurtSound;
    AudioSource deathSound;
    bool isDead = false;

    void Start(){
        enemyHealthbar.maxValue = targetHealth;
        enemyHealthbar.value = targetHealth;
        hurtSound = GetComponents<AudioSource>();
    }

    public void TakeTargetDamage (float amount) { 
        targetHealth -= amount;
        enemyHealthbar.value = targetHealth;
        
        if (giveMoneyOnDamage) {
            int randomAmount = Random.Range(minRandomHurtMoney, maxRandomHurtMoney);
            Player.ReceiveMoney(randomAmount);        
        }

        if (playHurtSound) {
            hurtSound[0].pitch = Random.Range(0.9f, 1.1f);
            if (!hurtSound[0].isPlaying) {
                hurtSound[0].Play();
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
            child.GetComponent<Rigidbody>().AddForce(-child.transform.forward * Gun.bulletForce / 15f, ForceMode.Impulse);
        }

        if (giveMoneyOnDeath) {
            int randomAmount = Random.Range(minRandomDeathMoney, maxRandomDeathMoney);
            Player.ReceiveMoney(randomAmount);
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
