using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private LayerMask shootableLayers;
    [SerializeField] private float gunRange;
    [SerializeField] private float swayAmount;
    [SerializeField] private float swaySpeed;
    [SerializeField] private float swaySmoothing;
    [SerializeField] private float swayMultiplier;
    private Vector3 originalEquipPoint;
    private GameObject gunObject;
    public ParticleSystem impactEffect;
    private GameObject muzzleFlashObject;
    private Animator animator;
    private AudioSource[] gunSounds;
    private AudioSource[] impactSounds;
    private float nextTimeToFire = 0;
    private bool gunEquipped = false;
    private float damage;
    private float fireRate;
    private float impactForce;
    private float clipAmmo = 1f;
    private float usedAmmo = 0f;
    private RaycastHit grab;
    public Player playerScript;
    private float cameraShakeMagnitude;
    private float cameraShakeDuration;
    private Vector3 orignalCameraPosition;
    private float recoilAngle;

    void Start() {
        originalEquipPoint = equipPoint.transform.localPosition;
        orignalCameraPosition = fpsCam.transform.localPosition;
    }

    void Update() {

        IdleSway();
        LookSway();

        if (gunEquipped == false && Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out grab, equipRange, gunLayers)) {   
            equipUI.text = "[E] Equip";
            equipUI.enabled = true;
            if (Input.GetKeyDown(KeyCode.E)) {
                gunObject = grab.transform.gameObject;
                gunSounds = gunObject.GetComponents<AudioSource>();
                muzzleFlashObject = gunObject.transform.Find("Muzzleflash").gameObject;
                animator = gunObject.transform.Find("Model").GetComponent<Animator>();
                Physics.IgnoreCollision(gunObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
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
                    recoilAngle = -70f;
                    cameraShakeDuration = 0.1f;
                    cameraShakeMagnitude = 1f;
                }
                if (gunObject.tag == "Rifle") {
                    damage = 5f;
                    fireRate = 5f;
                    impactForce = 100f;
                    clipAmmo = 16;
                    recoilAngle = -90f;
                    cameraShakeDuration = 0.2f;
                    cameraShakeMagnitude = 5f;
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
                Shoot();
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

    IEnumerator CameraShake() {
        float elapsed = 0f;
        while(elapsed < cameraShakeDuration) {
            elapsed += Time.deltaTime;
            float randomZ = Random.Range(-1f, 1f) * cameraShakeMagnitude;
            Vector3 shakePosition = new Vector3(fpsCam.transform.localPosition.x, fpsCam.transform.localPosition.y, randomZ);
            fpsCam.transform.localPosition = Vector3.Lerp(fpsCam.transform.localPosition, shakePosition, Time.deltaTime * 6f);
        }
        fpsCam.transform.localPosition = Vector3.Lerp(fpsCam.transform.localPosition, orignalCameraPosition, Time.deltaTime * 6f);
        yield return 0;
    }

    IEnumerator GunRecoil() {
        Quaternion targetRotation = Quaternion.Euler(recoilAngle, 0f, 0f);
        float elapsedTime = 0f;
        while (elapsedTime < 0.1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 6f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        elapsedTime = 0f;
        while (elapsedTime < 1) {
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRotation, Time.deltaTime * 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void IdleSway() { // TODO: change values in update instead of copying code
        if (gunEquipped) {    
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
                float walkSwayX = Mathf.Sin(Time.time * swaySpeed * 2f) * swayAmount;
                float walkSwayY = Mathf.Cos(Time.time * swaySpeed * 7f) * swayAmount * 2f;
                Vector3 walkSway = new Vector3(walkSwayX, walkSwayY, 0f);
                gunObject.transform.localPosition = Vector3.Lerp(gunObject.transform.localPosition, walkSway, Time.deltaTime * 6f);
                if (Input.GetKey("left shift") && playerScript.currentStamina > 0) {
                    float sprintSwayX = Mathf.Sin(Time.time * swaySpeed * 4f) * swayAmount;
                    float sprintSwayY = Mathf.Cos(Time.time * swaySpeed * 12f) * swayAmount * 6f;
                    Vector3 sprintSway = new Vector3(sprintSwayX, sprintSwayY, 0f);
                    gunObject.transform.localPosition = Vector3.Lerp(gunObject.transform.localPosition, sprintSway, Time.deltaTime * 6f);
                }
            } else {
                float idleSwayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
                float idleSwayY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;
                Vector3 idleSway = new Vector3(idleSwayX, idleSwayY, 0f);
                gunObject.transform.localPosition = Vector3.Lerp(gunObject.transform.localPosition, idleSway, Time.deltaTime * 6f);
            }
        }
    }

    void LookSway() {
        if (gunEquipped) {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRot = rotX * rotY;
            gunObject.transform.localRotation = Quaternion.Slerp(gunObject.transform.localRotation, targetRot, swaySmoothing * Time.deltaTime);
        }
    }

    void ProjectileShoot(){
        StartCoroutine("GunRecoil");
        StartCoroutine("CameraShake");
    }

    void Shoot() {
        RaycastHit shot;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out shot, gunRange, shootableLayers)) {
            if (!gunSounds[2].isPlaying) { // this is dumb but it works - cannot shoot when reloading
                Target target = shot.transform.GetComponent<Target>();
                ParticleSystem impactInstance = Instantiate(impactEffect, shot.point, Quaternion.LookRotation(shot.normal)); impactInstance.Play(); Destroy(impactInstance.gameObject, 2f);
                impactSounds = impactInstance.GetComponents<AudioSource>();
                muzzleFlashObject.GetComponent<ParticleSystem>().Play();
                gunSounds[0].pitch = Random.Range(0.8f, 1.2f); gunSounds[0].Play();
                StartCoroutine("GunRecoil");
                StartCoroutine("CameraShake");
                usedAmmo++;
                clipAmmoText.text = (clipAmmo - usedAmmo).ToString();

                if (target != null) {
                    target.TakeTargetDamage(damage);
                    impactSounds[0].pitch = Random.Range(0.9f, 1.1f); impactSounds[0].Play(); //flesh impact sound
                } else {
                    impactSounds[1].pitch = Random.Range(0.8f, 1.2f); impactSounds[1].Play(); //impact sound
                }

                if (shot.rigidbody != null) {
                    shot.rigidbody.AddForce(-shot.normal * impactForce);
                }
            }
        }
    }
}