using UnityEngine;

public class Bullet : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    private ParticleSystem particlesystem;
    private int collisionCount;
    [SerializeField] GameObject playerObject;

    void Start() {
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }

    void OnCollisionEnter(Collision collision) {
        collisionCount++;
        if (collisionCount == 1) {
            Target target = collision.gameObject.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeTargetDamage(gunScript.damage);
                particlesystem = fleshImpact;
            } else {
                particlesystem = impact;
            }

            Quaternion normalizedRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            ParticleSystem impactInstance = Instantiate(particlesystem, collision.contacts[0].point, normalizedRot);
            Destroy(impactInstance.gameObject, 3f); //should be enough time for sound to play/particles to despawn

            if (collision.gameObject.transform.GetComponent<Rigidbody>() != null) {
                collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(transform.forward * gunScript.impactForce, ForceMode.Impulse);
            } else {
                Instantiate(bulletHole, collision.contacts[0].point, normalizedRot);
            }
        } else {
            Destroy(gameObject);
        }
    }
}
