using UnityEngine;

public class ShootPlayer : MonoBehaviour {

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float aggroRadius = 10f;
    [SerializeField] private float bulletLifetime = 5f;
    [SerializeField] private ParticleSystem muzzleflash;
    [SerializeField] private GameObject shootPoint;
    private float timer;
    private AudioSource[] shootSound;

    void Start() {
        shootSound = GetComponents<AudioSource>();
    }

    void Update() {
        float distanceFromPlayer = Vector3.Distance (SpawnPlayer.playerInstance.transform.position, transform.position);
        if (distanceFromPlayer <= aggroRadius) {
            timer += Time.deltaTime;
            if (timer >= shootInterval) {
                timer = 0f;
                Shoot();
            }
        }
    }

    void Shoot() {
        shootSound[1].Play();
        muzzleflash.Play();
        Rigidbody bulletInstance = Instantiate(bulletPrefab, shootPoint.transform.position, transform.rotation);
        Vector3 directionToPlayer = (SpawnPlayer.playerInstance.transform.position - bulletInstance.transform.position).normalized;
        bulletInstance.AddForce(directionToPlayer * shootForce, ForceMode.Impulse);
        Destroy(bulletInstance.gameObject, bulletLifetime);
    }
}
