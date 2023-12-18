using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

    public Camera fpsCam;
    private float defaultFov;
    [SerializeField] private float adsFov = 60f;
    public Transform equipPoint;
    [SerializeField] private Transform adsPoint;
    public TextMeshProUGUI equipUI;
    public TextMeshProUGUI clipAmmoText;
    public Image crosshair;
    public float equipRange = 5;
    public LayerMask gunLayers;
    public LayerMask shootableLayers;
    public float gunRange = 1000f;
    private GameObject gunObject;
    public ParticleSystem impactEffect;
    private GameObject muzzleFlashObject;
    private Animator animator;
    private AudioSource[] gunSounds; //order: gunshot sound, no ammo sound, reload sound, cock sound
    private AudioSource[] impactSounds; //order: flesh impact, random impact sounds
    private float nextTimeToFire = 0;
    private bool gunEquipped = false;
    private float damage;
    private float fireRate;
    private float impactForce;
    private float clipAmmo = 1f;
    private float usedAmmo = 0f;
    private RaycastHit grab;
    public Player playerScript;

    void Start() {
        defaultFov = fpsCam.fieldOfView;
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
                playerScript.animator = animator;
                Physics.IgnoreCollision(gunObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
                grab.rigidbody.useGravity = false;
                grab.rigidbody.isKinematic = true;
                gunObject.transform.position = equipPoint.transform.position;
                gunObject.transform.rotation = equipPoint.transform.rotation;
                gunObject.transform.parent = equipPoint.transform;
                crosshair.enabled = true;
                playerScript.gunEquipped = true;
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

        if (gunEquipped == true && Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
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
            playerScript.gunEquipped = false;
            gunEquipped = false;
            usedAmmo = 0f;
        }

        if (gunEquipped && Input.GetButton("Fire2")) {
            gunObject.transform.position = adsPoint.transform.position;
            gunObject.transform.rotation = adsPoint.transform.rotation;
            fpsCam.fieldOfView = adsFov;
            
        } else if (gunEquipped && !gunSounds[2].isPlaying && !Input.GetButton("Fire2")) {
            gunObject.transform.position = equipPoint.transform.position;
            gunObject.transform.rotation = equipPoint.transform.rotation;
            fpsCam.fieldOfView = defaultFov;
        }
    }

    void Shoot() {
        RaycastHit shot;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out shot, gunRange, shootableLayers)) {
            if (!gunSounds[2].isPlaying) {
                usedAmmo++;
                Target target = shot.transform.GetComponent<Target>();
                clipAmmoText.text = (clipAmmo - usedAmmo).ToString();
                animator.SetTrigger("onShoot");
                gunSounds[0].Play();
                muzzleFlashObject.GetComponent<ParticleSystem>().Play();
                ParticleSystem impactInstance = Instantiate(impactEffect, shot.point, Quaternion.LookRotation(shot.normal)); impactInstance.Play(); Destroy(impactInstance.gameObject, 2f);
                impactSounds = impactInstance.GetComponents<AudioSource>();

                if (target != null) {
                    target.TakeTargetDamage(damage);
                    if(target.transform.tag == "Enemy") {
                        impactSounds[0].Play(); //play flesh impact sound -- target must have enemy tag
                    }
                } else if (target == null) {
                    int randomIndex = Random.Range(1, impactSounds.Length); //excludes 0 as that is flesh impact
                    impactSounds[randomIndex].Play(); //play random impact sounds
                }

                if (shot.rigidbody != null) {
                    shot.rigidbody.AddForce(-shot.normal * impactForce);
                }
            }
        }
    }
}
