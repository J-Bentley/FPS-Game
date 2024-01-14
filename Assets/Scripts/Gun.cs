using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Gun : MonoBehaviour {

    [SerializeField] private Camera fpsCam;
    [SerializeField] private float adsFov = 60f;
    [SerializeField] private float adsFovSpeed = 1f;
    [SerializeField] private Transform equipPoint;
    [SerializeField] private Transform equipPoint2;
    [SerializeField] private Transform adsPoint;
    [SerializeField] private TextMeshProUGUI equipUI;
    [SerializeField] private TextMeshProUGUI clipAmmoText;
    [SerializeField] private Image crosshair;
    [SerializeField] private float equipRange = 5;
    [SerializeField] private LayerMask gunLayers;
    [SerializeField] private LayerMask shootableLayers;
    [SerializeField] private float gunRange = 1000f;
    [SerializeField] private float swayAmount = 0.02f;
    [SerializeField] private float swaySpeed = 2f;
    [SerializeField] private float swaySmoothing;
    [SerializeField] private float swayMultiplier;
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
                grab.rigidbody.useGravity = false;
                grab.rigidbody.isKinematic = true;
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
                }

                if (gunObject.tag == "Rifle") {
                    damage = 5f;
                    fireRate = 5f;
                    impactForce = 100f;
                    clipAmmo = 16;
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

        if (gunEquipped && Input.GetButton("Fire2") && !Input.GetKey("left shift")) {
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, adsPoint.transform.localPosition, 6f * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, adsFov, adsFovSpeed * Time.deltaTime);
        } else if (gunEquipped && !Input.GetButton("Fire2")) {
            equipPoint.transform.localPosition = Vector3.Lerp(equipPoint.transform.localPosition, equipPoint2.transform.localPosition, 9f * Time.deltaTime);
        }
    }

    IEnumerator CameraShake(float duration, float magnitude) {
        Vector3 orignalPosition = fpsCam.transform.localPosition;
        float elapsed = 0f;
        while(elapsed < duration) {
            float randomX = Random.Range(-0.5f, 0.5f) * magnitude;
            float randomZ = Random.Range(-1f, 1f) * magnitude;
            fpsCam.transform.localPosition = new Vector3(randomX, fpsCam.transform.localPosition.y, randomZ);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        fpsCam.transform.localPosition = orignalPosition;
    }

    void IdleSway() {
        if (gunEquipped) {
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            float swayY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;
            Vector3 sway = new Vector3(swayX, swayY, 0);
            gunObject.transform.localPosition = equipPoint.transform.localPosition + sway;
        }
    }

    void LookSway() {
        if (gunEquipped) {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion targetRot = rotX * rotY;
            equipPoint.localRotation = Quaternion.Slerp(equipPoint.localRotation, targetRot, swaySmoothing * Time.deltaTime);
        }
    }

    void Shoot() {
        RaycastHit shot;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out shot, gunRange, shootableLayers)) {
            if (!gunSounds[2].isPlaying) { // this is dumb but it works - cannot shoot when reloading
                Target target = shot.transform.GetComponent<Target>();
                ParticleSystem impactInstance = Instantiate(impactEffect, shot.point, Quaternion.LookRotation(shot.normal)); impactInstance.Play(); Destroy(impactInstance.gameObject, 2f);
                impactSounds = impactInstance.GetComponents<AudioSource>();
                muzzleFlashObject.GetComponent<ParticleSystem>().Play();
                animator.SetTrigger("onShoot");
                clipAmmoText.text = (clipAmmo - usedAmmo).ToString();
                StartCoroutine(CameraShake(0.1f, 0.6f));
                gunSounds[0].pitch = Random.Range(0.8f, 1.2f);;
                gunSounds[0].Play();
                usedAmmo++;

                if (target != null) {
                    target.TakeTargetDamage(damage);
                    impactSounds[0].pitch = Random.Range(0.8f, 1.2f);;
                    impactSounds[0].Play(); //flesh impact sound
                } else {
                    impactSounds[1].pitch = Random.Range(0.8f, 1.2f);;
                    impactSounds[1].Play(); //impact sound
                }

                if (shot.rigidbody != null) {
                    shot.rigidbody.AddForce(-shot.normal * impactForce);
                }
            }
        }
    }
}