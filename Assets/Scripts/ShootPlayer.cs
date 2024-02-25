using System.Collections;
using UnityEngine;

public class ShootPlayer : MonoBehaviour {

    [SerializeField] private Rigidbody bulletPrefab;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float aggroRadius = 10f;
    [SerializeField] private ParticleSystem muzzleflash;
    [SerializeField] private float ammo;
    [SerializeField] private float reloadTime;
    private int usedAmmo;
    private float timer;
    private AudioSource[] audioSources;
    private Vector3 player;

    void Start() {
        audioSources = GetComponents<AudioSource>();
        player = SpawnPlayer.playerInstance.transform.position;
    }

    void Update() {
        float distanceFromPlayer = Vector3.Distance (player, transform.position);
        if (distanceFromPlayer <= aggroRadius) {
            timer += Time.deltaTime;
            if (timer >= shootInterval) {
                if (usedAmmo < ammo) {
                    timer = 0f;
                    Shoot();
                } else if (usedAmmo >= ammo) {
                    audioSources[2].Play();
                    StartCoroutine("Reload");
                }
            }
        }
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        usedAmmo = 0;
    }

    void Shoot() {
        usedAmmo++;
        audioSources[1].Play();
        muzzleflash.Play();
        Rigidbody bulletInstance = Instantiate(bulletPrefab, muzzleflash.transform.position, transform.rotation);
        Vector3 directionToPlayer = (SpawnPlayer.playerInstance.transform.position - bulletInstance.transform.position).normalized;
        bulletInstance.AddForce(directionToPlayer * shootForce, ForceMode.Impulse);
    }
}
