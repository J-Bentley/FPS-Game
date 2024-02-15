using UnityEngine;

public class ShootPlayer : MonoBehaviour {

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float aggroRadius = 10f;
    [SerializeField] private float bulletLifetime = 5f;
    private float timer;

    void Update() {
        float distanceFromPlayer = Vector3.Distance (playerTransform.transform.position, transform.position);
        if (distanceFromPlayer <= aggroRadius) {
            timer += Time.deltaTime;
            if (timer >= shootInterval) {
                timer = 0f;
                Shoot();
            }
        }
    }

    void Shoot() {
        Rigidbody bulletInstance = Instantiate(bulletPrefab, transform.GetChild(1).position, transform.rotation);
        Vector3 directionToPlayer = (playerTransform.position - bulletInstance.transform.position).normalized;
        bulletInstance.AddForce(directionToPlayer * shootForce, ForceMode.Impulse);
        Destroy(bulletInstance.gameObject, bulletLifetime);
    }
}
