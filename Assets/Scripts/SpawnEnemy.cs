using UnityEngine;

// spawns enemy at random child when player is in trigger, stops spawning when player leaves -- enemies will follow between areas

public class SpawnEnemy : MonoBehaviour {

    [SerializeField] float spawnInterval;
    [SerializeField] GameObject enemy;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InvokeRepeating("Spawn", spawnInterval, spawnInterval);
            Debug.Log("Entered " + transform.name);
        }
    }

    void Spawn() {
        int childCount = transform.childCount;
        int randomIndex = Random.Range(0, childCount);
        Transform randomChild = transform.GetChild(randomIndex);
        Instantiate(enemy, randomChild);
        Debug.Log("Enemy spawned at " + randomChild.name + " in " + transform.name);
    }

    void OnTriggerExit() {
        CancelInvoke("Spawn");
        Debug.Log("Exited " + transform.name);
    }
}
