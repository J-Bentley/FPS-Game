using UnityEngine;

public class Bullet : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] ParticleSystem impactEffect;
    private float collisionCount = 0f;
    private AudioSource[] impactSounds;

    void OnCollisionEnter(Collision collision) {
        if(collisionCount == 0) {
            ParticleSystem impactEffectInstance = Instantiate(impactEffect, collision.contacts[0].point, Quaternion.identity); impactEffectInstance.Play(); Destroy(impactEffectInstance.gameObject, 2f);
            impactSounds = impactEffectInstance.GetComponents<AudioSource>();
            Target target = collision.gameObject.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeTargetDamage(gunScript.damage);
                impactSounds[0].pitch = Random.Range(0.9f, 1.1f);
                impactSounds[0].Play(); //flesh impact sound
            } else {
                impactSounds[1].pitch = Random.Range(0.8f, 1.2f);
                impactSounds[1].Play(); //impact sound
            }
            if (collision.transform.GetComponent<Rigidbody>() != null) {
                collision.transform.GetComponent<Rigidbody>().AddForce(transform.forward * gunScript.impactForce, ForceMode.Impulse);
            }
        }
        collisionCount++;
    }
}
