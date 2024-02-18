using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private bool giveMoneyOnDeath = true;
    [SerializeField] private bool destroyPeices = false;
    [SerializeField] private bool playHurtSound = false;
    [SerializeField] private bool playDeathSound = false;
    [SerializeField] private float destroyPeicesTimer;
    [SerializeField] private float targetHealth;
    [SerializeField] private int minRandomMoney;
    [SerializeField] private int maxRandomMoney;
    [SerializeField] private Slider enemyHealthbar = null;
    private AudioSource[] hurtSound;
    private AudioSource deathSound;
    private bool isDead = false;

    void Start(){
        enemyHealthbar.maxValue = targetHealth;
        enemyHealthbar.value = targetHealth;
        hurtSound = GetComponents<AudioSource>();
    }

    public void TakeTargetDamage (float amount) { 
        targetHealth -= amount;
        enemyHealthbar.value = targetHealth;

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
            child.GetComponent<Rigidbody>().AddForce(-child.transform.forward * Gun.bulletForce / 10f, ForceMode.Impulse);
        }

        if (giveMoneyOnDeath) {
            int randomAmount = Random.Range(minRandomMoney, maxRandomMoney);
            playerScript.ReceiveMoney(randomAmount);
        }

        if (playDeathSound) {
            deathSound = destroyedObject.GetComponent<AudioSource>(); 
            deathSound.pitch = Random.Range(0.9f, 1.1f);
            if(!deathSound.isPlaying) {
                deathSound.Play();
            }
        }

        if(destroyPeices) {
            Destroy(destroyedObject, destroyPeicesTimer);
        }
    }
}
