using UnityEngine;

public class HurtPlayerOnTouch : MonoBehaviour {

    public Player playerScript;
    [SerializeField] private float damage = 10f;

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            playerScript.TakeDamage(damage);
        }
    }
}
