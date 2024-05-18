using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    GameObject enemy;
    float timer;
    [SerializeField] GameObject pistolEnemy;
    [SerializeField] GameObject rifleEnemy;
    [SerializeField] GameObject sniperEnemy;
    [SerializeField] GameObject shotgunEnemy;
    [SerializeField] float spawnInterval = 10f;

    void Update() {
        timer += Time.deltaTime;
        if (timer >= spawnInterval) {
            timer = 0f;
            float randomEnemyIndex = Random.Range (0,2);
            int randomSpawnIndex = Random.Range (0, transform.childCount - 1);
            switch(randomEnemyIndex) {
                case 0:
                    enemy = pistolEnemy;
                    break;
                case 1:
                    enemy = rifleEnemy;
                    break;
                case 2:
                    enemy = sniperEnemy;
                    break;
                default:
                    break;
            }
            Instantiate(enemy, transform.GetChild(randomSpawnIndex).position,  Quaternion.identity);
            Debug.Log("Spawned an enemy!");
        }
    }
}
