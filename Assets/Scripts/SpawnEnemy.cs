using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    [SerializeField] private GameObject pistolEnemy;

    void Start() {
        Instantiate(pistolEnemy, gameObject.transform.position, Quaternion.identity);
    }
}
