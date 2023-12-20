using UnityEngine;

public class ShootPlayer : MonoBehaviour {

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootForce = 100f;
    private float timer;

    void Start() {

    }

    void Update() {
        timer += Time.deltaTime;
        Debug.Log(timer);
        if(timer >= shootInterval) {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot() {
        Rigidbody bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Vector3 directionToPlayer = (playerTransform.position - bulletInstance.transform.position).normalized;
        bulletInstance.AddForce(directionToPlayer * shootForce, ForceMode.Impulse);
    }
}
