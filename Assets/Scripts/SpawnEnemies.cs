using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    public GameObject enemyObject;
    private Transform[] spawnPoints;
    public float spawnInterval = 10f;
    private float handyTimer = 0;

    void Start() {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update() {
        handyTimer += Time.deltaTime;
        if (handyTimer >= spawnInterval){
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyObject, spawnPoints[randomIndex].position, Quaternion.identity);
            handyTimer = 0;
        }
    }
}
