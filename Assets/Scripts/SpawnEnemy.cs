using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    [SerializeField] private GameObject enemy;

    void Start() {
        Instantiate(enemy, gameObject.transform.position, Quaternion.identity);
    }
}
