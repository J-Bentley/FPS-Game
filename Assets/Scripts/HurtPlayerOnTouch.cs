
using UnityEngine;

public class HurtPlayerOnTouch : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private float attackDamage = 10f;

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.TakeDamage(attackDamage);
        }
    }
}
