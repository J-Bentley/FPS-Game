using UnityEngine;
using TMPro;

public class SpawnEnemies : MonoBehaviour {

    [SerializeField] private GameObject runnerEnemy;
    [SerializeField] private GameObject shooterEnemy;
    private Transform[] spawnPoints;
    private int wave = 1;
    private int enemyAmount = 2;
    public static int enemiesKilled = 0;
    [SerializeField] private TextMeshProUGUI waveText;

    void Start() {
        spawnPoints = GetComponentsInChildren<Transform>();
        waveText.text = "Wave: " + wave + " (" + enemyAmount + " enemies)";
        SpawnEnemy();
    }

    void Update() {
        if (enemiesKilled == enemyAmount) {
            wave ++;
            enemiesKilled = 0;
            enemyAmount += 2;
            waveText.text = "Wave: " + wave + " (" + enemyAmount + " enemies)";
            Invoke("SpawnEnemy", 5f);
        }
    }

    void SpawnEnemy() {
        for (int i = 0; i < enemyAmount; i++) {
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            int randomEnemy = Random.Range(0, 2);
            if (randomEnemy == 0) {
                Instantiate(shooterEnemy, spawnPoints[randomSpawn].transform.position, spawnPoints[randomSpawn].transform.rotation);
            } else if (randomEnemy == 1) {
                Instantiate(runnerEnemy, spawnPoints[randomSpawn].transform.position, spawnPoints[randomSpawn].transform.rotation);
            }
        }
    }
}
