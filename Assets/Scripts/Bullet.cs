using UnityEngine;

public class Bullet : MonoBehaviour {
    public Gun gunScript;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    private ParticleSystem particlesystem;
    private int collisionCount;

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
        
            Quaternion correctRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            ParticleSystem impactInstance = Instantiate(particlesystem, collision.contacts[0].point, correctRot);
            impactInstance.GetComponent<AudioSource>().Play();
            Destroy(impactInstance.gameObject, 3f);
        
            if (collision.transform.GetComponent<Rigidbody>() != null) {
                collision.transform.GetComponent<Rigidbody>().AddForce(transform.forward * gunScript.impactForce, ForceMode.Impulse);
            }
        } else {
            Destroy(gameObject);
        }
    }
}
