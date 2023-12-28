using UnityEngine;
using TMPro;

public class SpawnEnemies : MonoBehaviour {

    [SerializeField] private GameObject runnerEnemy;
    [SerializeField] private GameObject shooterEnemy;
    private GameObject enemyObject;
    private Transform[] spawnPoints;
    private int wave = 1;
    private int enemiesToSpawnThisRound = 2;
    public static int enemiesKilledThisRound = 0; //is incremented by target script
    private AudioSource[] audioSource;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private float timeBetweenWaves = 10f;
    [SerializeField] private CharacterController playerController;

    void Start() {
        spawnPoints = GetComponentsInChildren<Transform>();
        audioSource = playerController.GetComponents<AudioSource>();
        waveText.text = "Wave: " + wave;
        SpawnEnemy();
    }

    void Update() {
        if (enemiesKilledThisRound == enemiesToSpawnThisRound) {
            wave ++;
            enemiesKilledThisRound = 0;
            enemiesToSpawnThisRound += 2;
            waveText.text = "Another wave approaches...";
            audioSource[7].Play(); // wave complete sound
            Invoke("SpawnEnemy", timeBetweenWaves);
        }
    }

    void SpawnEnemy() {
        waveText.text = "Wave: " + wave;
        for (int i = 0; i < enemiesToSpawnThisRound; i++) {
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            int randomEnemy = Random.Range(0, 2);
            if (randomEnemy == 0) {
                enemyObject = shooterEnemy;
            } else if (randomEnemy == 1) {
                enemyObject = runnerEnemy;
            }
            Instantiate(enemyObject, spawnPoints[randomSpawn].transform.position, spawnPoints[randomSpawn].transform.rotation);
        }
    }
}
