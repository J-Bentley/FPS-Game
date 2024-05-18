using System.Collections;
using UnityEngine;

public class ShootPlayer : MonoBehaviour {

    [SerializeField] Rigidbody bulletPrefab;
    [SerializeField] float shootForce = 100f;
    [SerializeField] float aggroRadius = 10f;
    [SerializeField] ParticleSystem muzzleflash;
    [SerializeField] float shootInterval;
    float timer;
    AudioSource[] audioSources;
    Vector3 player;

    void Start() {
        audioSources = GetComponents<AudioSource>();
        player = SpawnPlayer.playerInstance.transform.position;
    }

    void Update() {
        float distanceFromPlayer = Vector3.Distance (player, transform.position);
        if (distanceFromPlayer <= aggroRadius) {
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
