using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private float damage = 10f;

    void OnTriggerEnter(Collider collider) {
        if(collider.transform.tag == "Player") {
            playerScript.TakeDamage(damage);
        }
    }

    void OnTriggerStay(Collider collider) {
        if(collider.transform.tag == "Player") {
            playerScript.TakeDamage(damage);
        }
    }
}
