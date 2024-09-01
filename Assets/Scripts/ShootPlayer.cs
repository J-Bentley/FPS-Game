using UnityEngine;
 /// <summary>
 /// DEPRECATED
 /// </summary>
public class ShootPlayer : MonoBehaviour {

    [SerializeField] Rigidbody bulletPrefab;
    [SerializeField] float shootForce = 100f;
    [SerializeField] float aggroDistance = 10f;
    ParticleSystem muzzleflash;
    [SerializeField] float shootInterval;
    float timer;
    AudioSource[] audioSources;

    void Start() {
        audioSources = GetComponents<AudioSource>();
    }

    void Update() {
        Transform playerTransform = SpawnPlayer.playerInstance.transform;
        float distanceFromPlayer = Vector3.Distance (playerTransform.position, transform.position);

        if (distanceFromPlayer <= aggroDistance) {
            timer += Time.deltaTime;
            if (timer >= shootInterval) {
                timer = 0f;
                Shoot();
            }
        }
    }

    void Shoot() {
        audioSources[1].Play();
        muzzleflash.Play();
        Rigidbody bulletInstance = Instantiate(bulletPrefab, muzzleflash.transform.position, transform.rotation);
        Vector3 directionToPlayer = (SpawnPlayer.playerInstance.transform.position - bulletInstance.transform.position).normalized;
        bulletInstance.AddForce(directionToPlayer * shootForce, ForceMode.Impulse);
    }
}
