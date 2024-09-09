using UnityEngine;

public class Healthpack : MonoBehaviour {

    [SerializeField] float healingAmount = 50f;
    [SerializeField] bool destroyAfterUse = true;

    void OnTriggerEnter (Collider collider) {
        if (collider.gameObject.transform.tag == "Player") {
            if (Player.instance.currentHealth != Player.instance.maxHealth) {
                Player.instance.ReceiveHealing(healingAmount);

                if (destroyAfterUse) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
