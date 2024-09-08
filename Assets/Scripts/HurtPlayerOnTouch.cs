using UnityEngine;

public class HurtPlayerOnTouch : MonoBehaviour {

    [SerializeField] private float damage;

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            Player.instance.TakeDamage(damage);
        }
    }
}
