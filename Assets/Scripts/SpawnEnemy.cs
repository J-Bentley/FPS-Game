using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    [SerializeField] GameObject enemy;
    [SerializeField] float spawnInterval;


    void Start() {
        InvokeRepeating("Spawn", spawnInterval, spawnInterval);
    }

    void Spawn() {
        Instantiate(enemy, transform.position, Quaternion.identity);
        Debug.Log("Spawned enemy at " + gameObject.name);
    }
}
