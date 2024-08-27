using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] float spawnInterval;


    void Start() {
        InvokeRepeating("SpawnRandomEnemy", spawnInterval, spawnInterval);
    }

    void SpawnRandomEnemy() {
        int childCount = transform.childCount;
        if (childCount == 0) {
            Debug.LogWarning("No children to spawn enemy at!");
            return;
        }
        int randomIndex = Random.Range(0, childCount);
        Transform randomChild = transform.GetChild(randomIndex);
        Instantiate(enemy, randomChild.position, Quaternion.identity);
        Debug.Log("Spawned enemy at index " + randomIndex);
    }
}
