using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Target : MonoBehaviour {

    [SerializeField] private float targetHealth = 50f;
    [SerializeField] private GameObject destroyedVersion;
    [SerializeField] private bool destroyPeices = false;
    [SerializeField] private float destroyPeicesTimer = 10;
    [SerializeField] private bool playHurtSound = false;
    [SerializeField] private bool playDeathSound = false;
    [SerializeField] private Slider enemyHealthbar = null;
    [SerializeField] private GameObject staminapack;
    [SerializeField] private bool spawnStaminapackOnDeath = true;
    [SerializeField] private bool giveMoneyOnDeath = true;
    [SerializeField] private int minRandomMoney = 10;
    [SerializeField] private int maxRandomMoney = 30;
    [SerializeField] private TextMeshProUGUI moneyText;
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
        SpawnEnemies.enemiesKilledThisRound++;
        Destroy(gameObject);
        GameObject destroyedGO = Instantiate(destroyedVersion, transform.position, transform.rotation);

        if (spawnStaminapackOnDeath) {
            Instantiate(staminapack, transform.position, transform.rotation);
        }

        if (giveMoneyOnDeath) {
            int randomAmount = Random.Range(minRandomMoney, maxRandomMoney);
            Player.money += randomAmount;
            moneyText.text = "$" + Player.money;
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
