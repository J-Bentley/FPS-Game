using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    [SerializeField] private GameObject runnerEnemy;
    [SerializeField] private GameObject shooterEnemy;
    private Transform[] spawnPoints;
    public float spawnInterval = 10f;
    private float timer = 0;

    void Start() {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnInterval){
            timer = 0;
            int randomSpawnpoint = Random.Range(0, spawnPoints.Length);
            int randomEnemy = Random.Range(0, 2);
            if (randomEnemy == 0) {
                Instantiate(runnerEnemy, spawnPoints[randomSpawnpoint].position, Quaternion.identity);
            } else if (randomEnemy == 1) {
                Instantiate(shooterEnemy, spawnPoints[randomSpawnpoint].position, Quaternion.identity);
            }
        }
    }
}
