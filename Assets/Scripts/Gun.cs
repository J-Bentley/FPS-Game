using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour {
    [SerializeField] Camera fpsCam;
    [SerializeField] float adsFov;
    [SerializeField] float sniperFov;
    [SerializeField] Transform equipPoint;
    [SerializeField] Transform adsPoint;
    [SerializeField] TextMeshProUGUI gunText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] Image crosshair;
    [SerializeField] Image scopeOverlay;
    [SerializeField] float equipRange;
    [SerializeField] LayerMask gunLayers;
    public static GameObject gunObject;
    public static bool gunEquipped = false;
    public static float damage;
    public static float bulletForce;
    float originalFov;
    Vector3 originalEquipPoint;
    ParticleSystem muzzleFlash;
    Animator animator;
    AudioSource[] gunSounds;
    float nextTimeToFire = 0;
    float fireRate;
    float recoilAngle;
    float clipAmmo = 1f;
    float usedAmmo = 0f;
    RaycastHit grab;
    float maxDistance = 100f;

    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    [SerializeField] ParticleSystem fleshBulletHole;
    ParticleSystem impactParticlesystem;
    ParticleSystem bulletHoleParticlesystem;

    void Start() {
        originalEquipPoint = equipPoint.transform.localPosition;
        originalFov = fpsCam.fieldOfView;

    }

    void Update() {
        if (!gunEquipped && Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out grab, equipRange, gunLayers)) {   
            gunText.text = "[E] Equip";
            gunText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                gunObject = grab.transform.gameObject;
                gunSounds = gunObject.GetComponents<AudioSource>();
                muzzleFlash = gunObject.transform.GetComponentInChildren<ParticleSystem>();
                animator = gunObject.transform.GetComponentInChildren<Animator>();
                gunObject.GetComponent<Collider>().enabled = false;                
                gunObject.GetComponent<Rigidbody>().useGravity = false;
                gunObject.GetComponent<Rigidbody>().isKinematic = true;
                gunObject.transform.position = equipPoint.transform.position;
                gunObject.transform.rotation = equipPoint.transform.rotation;
                gunObject.transform.parent = equipPoint.transform;
                crosshair.enabled = true;
                gunEquipped = true;
                gunSounds[3].Play(); //cock sound
                switch (gunObject.tag) {
                    case "Pistol":
                        damage = 10f;
                        fireRate = 2f;
                        clipAmmo = 5f;
                        bulletForce = 150f;
                        recoilAngle = -30f;
                        break;
                    case "Rifle":
                        damage = 5f;
                        fireRate = 5f;
                        clipAmmo = 16f;
                        bulletForce = 200f;
                        recoilAngle = -30f;
                        break;
                    case "Sniper":
                        damage = 20f;
                        fireRate = 1f;
                        clipAmmo = 3f;
                        bulletForce = 300f;
                        recoilAngle = -40f;
                        break;
                    case "Shotgun":
                        damage = 10f;
                        fireRate = 1f;
                        clipAmmo = 2f;
                        bulletForce = 200f;
                        recoilAngle = -90f;
                        break;
                    default:
                        break;
                }
                ammoText.enabled = true;
                ammoText.text = clipAmmo.ToString();
            }
        } else {
            gunText.enabled = false;
        }

        if (!GameManager.gamePaused && gunEquipped == true && Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            if (usedAmmo < clipAmmo) {
                if (!gunSounds[2].isPlaying) { 
                    Shoot();
                    ammoText.text = (clipAmmo - usedAmmo).ToString();
                }
            } 
        }
        
        if (usedAmmo == clipAmmo) {
            gunText.enabled = true;
            gunText.text = "[R] Reload";

            if (Input.GetButton("Fire1")) {
                gunSounds[1].Play(); //no ammo sound
            }
        }

        if (usedAmmo > 0 && Input.GetKeyDown(KeyCode.R)) {
            usedAmmo = 0f;
            gunText.enabled = false;
            gunSounds[2].Play(); //reload sound
            animator.SetTrigger("onReload");
            ammoText.text = clipAmmo.ToString();
        }

        // Aiming Down Sights -- at least it works?
        // should refactor this into an FOV class so this class and player can dynamically change fov when sprinting/ADS and go between different values smoothly
        if (gunEquipped && Input.GetButton("Fire2")) {
            crosshair.enabled = false;
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, adsPoint.transform.localPosition, 6f * Time.deltaTime);
            if (gunObject.transform.tag == "Sniper") {
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, sniperFov, Time.deltaTime * 6f);
                gunObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                scopeOverlay.enabled = true;
            } else {
                fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, adsFov, Time.deltaTime * 6f);
            }
        } else if (gunEquipped && !Input.GetButton("Fire2")) {
            crosshair.enabled = true;
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, originalEquipPoint, 10f * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, originalFov, Time.deltaTime * 6f);
            if (gunObject.transform.tag == "Sniper") {
                gunObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                scopeOverlay.enabled = false;
            }
        }

        //Dropping Gun
        if (gunEquipped && Input.GetKeyDown(KeyCode.F)) {
            if (gunObject.transform.tag == "Sniper") {
                gunObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                scopeOverlay.enabled = false;
            }
            crosshair.enabled = false;
            ammoText.enabled = false;
            gunObject.GetComponent<Collider>().enabled = true; 
            gunObject.transform.parent = null;
            grab.rigidbody.useGravity = true;
            grab.rigidbody.isKinematic = false;
            gunEquipped = false;
            usedAmmo = 0f;
            gunObject.GetComponent<Rigidbody>().AddForce(equipPoint.transform.forward * 10f, ForceMode.Impulse);
            //gunObject = null;
            StartCoroutine(changeFov(originalFov)); // resets fov if gun is dropped while ADS
        }
    }

    void Shoot() {
        usedAmmo++;
        gunSounds[0].pitch = Random.Range(0.8f, 1.2f);
        gunSounds[0].Play();
        muzzleFlash.Play();
        StartCoroutine("Recoil");

        RaycastHit shot;
        Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out shot, maxDistance);
        Target target = shot.transform.GetComponent<Target>();
        
        if (target != null) {
            target.TakeTargetDamage(damage);
            impactParticlesystem = fleshImpact;
            bulletHoleParticlesystem = fleshBulletHole;
        } else {
            impactParticlesystem = impact;
            bulletHoleParticlesystem = bulletHole;
        }
        Instantiate(impactParticlesystem, shot.point, Quaternion.identity);
        ParticleSystem bulletHoleInstance = Instantiate(bulletHoleParticlesystem, shot.point, Quaternion.identity);
        bulletHoleInstance.transform.parent = shot.transform;
    }

    IEnumerator changeFov(float newFov) {
        float elapsedTime = 0f;
        while (elapsedTime < 1) {
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, newFov, Time.deltaTime * 6f);
            elapsedTime += Time.deltaTime;
        }
        yield return null;
    }
    
 // fuck it.
    IEnumerator Recoil() {
        Quaternion targetRotation = Quaternion.Euler(gunObject.transform.localRotation.x + recoilAngle, gunObject.transform.localRotation.y, gunObject.transform.localRotation.z);
        float elapsedTime = 0f;
        while (elapsedTime < 0.1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        targetRotation = Quaternion.Euler(gunObject.transform.localRotation.x, gunObject.transform.localRotation.y, gunObject.transform.localRotation.z);
        elapsedTime = 0f;
        while (elapsedTime < 1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}