using UnityEngine;

public class HurtPlayerOnTouch : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private float damage = 10f;

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            playerScript.TakeDamage(damage);
        }
    }
}
