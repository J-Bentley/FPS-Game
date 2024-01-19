using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gun : MonoBehaviour {
    [SerializeField] private Camera fpsCam;
    [SerializeField] private float adsFov;
    [SerializeField] private float adsFovSpeed;
    [SerializeField] private Transform equipPoint;
    [SerializeField] private Transform adsPoint;
    [SerializeField] private TextMeshProUGUI equipUI;
    [SerializeField] private TextMeshProUGUI clipAmmoText;
    [SerializeField] private Image crosshair;
    [SerializeField] private float equipRange;
    [SerializeField] private LayerMask gunLayers;
    [SerializeField] private GameObject bullet;
    private Vector3 originalEquipPoint;
    public GameObject gunObject;
    private GameObject muzzleFlashObject;
    private Animator animator;
    private AudioSource[] gunSounds;
    private float nextTimeToFire = 0;
    public bool gunEquipped = false;
    public float damage;
    private float fireRate;
    public float impactForce;
    private float bulletForce;
    private float clipAmmo = 1f;
    private float usedAmmo = 0f;
    private RaycastHit grab;
    public Player playerScript;

    void Start() {
        originalEquipPoint = equipPoint.transform.localPosition;
    }

    void Update() {
        if (gunEquipped == false && Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out grab, equipRange, gunLayers)) {   
            equipUI.text = "[E] Equip";
            equipUI.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                gunObject = grab.transform.gameObject;
                gunSounds = gunObject.GetComponents<AudioSource>();
                muzzleFlashObject = gunObject.transform.Find("Muzzleflash").gameObject;
                animator = gunObject.transform.Find("Model").GetComponent<Animator>();
                gunObject.GetComponent<Collider>().enabled = false;                
                gunObject.GetComponent<Rigidbody>().useGravity = false;
                gunObject.GetComponent<Rigidbody>().isKinematic = true;
                gunObject.transform.position = equipPoint.transform.position;
                gunObject.transform.rotation = equipPoint.transform.rotation;
                gunObject.transform.parent = equipPoint.transform;
                crosshair.enabled = true;
                gunEquipped = true;
                gunSounds[3].Play(); //cock sound

                if (gunObject.tag == "Pistol") {
                    damage = 10f;
                    fireRate = 2f;
                    impactForce = 50f;
                    clipAmmo = 6f;
                    bulletForce = 50f;
                }
                if (gunObject.tag == "Rifle") {
                    damage = 5f;
                    fireRate = 5f;
                    impactForce = 100f;
                    clipAmmo = 16;
                    bulletForce = 80f;
                }
                clipAmmoText.enabled = true;
                clipAmmoText.text = clipAmmo.ToString();
            }
        } else {
            equipUI.enabled = false;
        }

        if (!GameManager.gamePaused && gunEquipped == true && Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            if (usedAmmo < clipAmmo) {
                if (!gunSounds[2].isPlaying) {
                    Shoot();
                }
            } 
        }

        if (usedAmmo > 0 && Input.GetKeyDown(KeyCode.R)) {
            usedAmmo = 0f;
            equipUI.enabled = false;
            gunSounds[2].Play(); //reload sound
            animator.SetTrigger("onReload");
            clipAmmoText.text = clipAmmo.ToString();
        }

        if (usedAmmo == clipAmmo) {
            equipUI.enabled = true;
            equipUI.text = "[R] Reload";

            if (Input.GetButton("Fire1")) {
                gunSounds[1].Play(); //no ammo sound
            }
        }

        if (gunEquipped && Input.GetKeyDown(KeyCode.F)) {
            crosshair.enabled = false;
            clipAmmoText.enabled = false;
            gunObject.GetComponent<Collider>().enabled = true; 
            gunObject.transform.parent = null;
            grab.rigidbody.useGravity = true;
            grab.rigidbody.isKinematic = false;
            gunEquipped = false;
            usedAmmo = 0f;
        }

        if (gunEquipped && Input.GetButton("Fire2")) {
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, adsPoint.transform.localPosition, 6f * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, adsFov, adsFovSpeed * Time.deltaTime);
        } else if (gunEquipped && !Input.GetButton("Fire2")) {
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, originalEquipPoint, 6f * Time.deltaTime);
        }
    }

    void Shoot(){
        GameObject bulletInstance = Instantiate(bullet, muzzleFlashObject.transform.position, Quaternion.LookRotation(transform.forward));
        bulletInstance.GetComponent<Rigidbody>().AddForce(gunObject.transform.forward * bulletForce, ForceMode.Impulse);
        Destroy(bulletInstance, 8f);
        usedAmmo++;
        clipAmmoText.text = (clipAmmo - usedAmmo).ToString();
        gunSounds[0].pitch = Random.Range(0.8f, 1.2f);
        gunSounds[0].Play();
        muzzleFlashObject.GetComponent<ParticleSystem>().Play();
    }
}
    /*void RaycastShoot() {
        RaycastHit shot;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out shot, gunRange, shootableLayers)) {
            if (!gunSounds[2].isPlaying) { // dumb but it works - cannot shoot when reloading
                Target target = shot.transform.GetComponent<Target>();
                ParticleSystem impactInstance = Instantiate(impactEffect, shot.point, Quaternion.LookRotation(shot.normal));
                impactInstance.Play();
                Destroy(impactInstance.gameObject, 2f);
                impactSounds = impactInstance.GetComponents<AudioSource>();
                muzzleFlashObject.GetComponent<ParticleSystem>().Play();
                gunSounds[0].pitch = Random.Range(0.8f, 1.2f);
                gunSounds[0].Play();

                usedAmmo++;
                clipAmmoText.text = (clipAmmo - usedAmmo).ToString();

                if (target != null) {
                    target.TakeTargetDamage(damage);
                    impactSounds[0].pitch = Random.Range(0.9f, 1.1f);
                    impactSounds[0].Play(); //flesh impact sound
                } else {
                    impactSounds[1].pitch = Random.Range(0.8f, 1.2f);
                    impactSounds[1].Play(); //impact sound
                }

                if (shot.rigidbody != null) {
                    shot.rigidbody.AddForce(-shot.normal * impactForce);
                }
            }
        }
    } */