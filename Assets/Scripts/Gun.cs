using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour {
    [SerializeField] private Camera fpsCam;
    [SerializeField] private float adsFov;
    [SerializeField] private Transform equipPoint;
    [SerializeField] private Transform adsPoint;
    [SerializeField] private TextMeshProUGUI equipUI;
    [SerializeField] private TextMeshProUGUI clipAmmoText;
    [SerializeField] private Image crosshair;
    [SerializeField] private Image scopeOverlay;
    [SerializeField] private float equipRange;
    [SerializeField] private LayerMask gunLayers;
    [SerializeField] private GameObject rifleBullet;
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private float sniperFov;
    private GameObject bullet;
    private Vector3 originalEquipPoint;
    public GameObject gunObject;
    private GameObject muzzleFlashObject;
    private Animator animator;
    private AudioSource[] gunSounds;
    private float nextTimeToFire = 0;
    public bool gunEquipped = false;
    public static float damage;
    private float fireRate;
    public static float bulletForce;
    private float recoilAngle;
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
                    clipAmmo = 6f;
                    bulletForce = 150f;
                    recoilAngle = -50f;
                    bullet = pistolBullet;
                }
                if (gunObject.tag == "Rifle") {
                    damage = 5f;
                    fireRate = 5f;
                    clipAmmo = 16f;
                    bulletForce = 200f;
                    recoilAngle = -70f;
                    bullet = rifleBullet;
                }
                if (gunObject.tag == "Sniper"){
                    damage = 20f;
                    fireRate = 1f;
                    clipAmmo = 4f;
                    bulletForce = 300f;
                    recoilAngle = -80f;
                    bullet = rifleBullet;
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
                    clipAmmoText.text = (clipAmmo - usedAmmo).ToString();
                }
            } 
        }
        
        if (usedAmmo == clipAmmo) {
            equipUI.enabled = true;
            equipUI.text = "[R] Reload";

            if (Input.GetButton("Fire1")) {
                gunSounds[1].Play(); //no ammo sound
            }
        }

        if (usedAmmo > 0 && Input.GetKeyDown(KeyCode.R)) {
            usedAmmo = 0f;
            equipUI.enabled = false;
            gunSounds[2].Play(); //reload sound
            animator.SetTrigger("onReload");
            clipAmmoText.text = clipAmmo.ToString();
        }

        if (gunEquipped && Input.GetButton("Fire2")) {
            crosshair.enabled = false;
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, adsPoint.transform.localPosition, 6f * Time.deltaTime);
            if (gunObject.transform.tag == "Sniper") {
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, sniperFov, 6f * Time.deltaTime);
                gunObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                scopeOverlay.enabled = true;
            } else {
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, adsFov, 6f * Time.deltaTime);
            }
        } else if (gunEquipped && !Input.GetButton("Fire2")) {
            crosshair.enabled = true;
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, originalEquipPoint, 10f * Time.deltaTime);
            if (gunObject.transform.tag == "Sniper") {
                gunObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                scopeOverlay.enabled = false;
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
            gunObject.GetComponent<Rigidbody>().AddForce(equipPoint.transform.forward * 10f, ForceMode.Impulse);
            gunObject = null;
        }
    }

    void Shoot() {
        //StartCoroutine("Recoil");
        GameObject bulletInstance = Instantiate(bullet, muzzleFlashObject.transform.position, Quaternion.LookRotation(muzzleFlashObject.transform.forward));
        Physics.IgnoreCollision(bulletInstance.GetComponent<Collider>(), GetComponent<Collider>(), true);
        bulletInstance.GetComponent<Rigidbody>().AddForce(muzzleFlashObject.transform.forward * bulletForce, ForceMode.Impulse);
        usedAmmo++;
        gunSounds[0].pitch = Random.Range(0.8f, 1.2f);
        gunSounds[0].Play();
        muzzleFlashObject.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator Recoil() {
        Quaternion targetRotation = Quaternion.Euler(gunObject.transform.localRotation.x + recoilAngle, gunObject.transform.localRotation.y, gunObject.transform.localRotation.z);
        float elapsedTime = 0f;
        while (elapsedTime < 0.1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 6f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        targetRotation = Quaternion.Euler(gunObject.transform.localRotation.x, gunObject.transform.localRotation.y, gunObject.transform.localRotation.z);
        elapsedTime = 0f;
        while (elapsedTime < 1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}