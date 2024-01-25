using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    private ParticleSystem particlesystem;
    [SerializeField] CharacterController playerObject;
    private int collisionCount;

    void OnCollisionEnter(Collision collision) {
        collisionCount++;
        if (collisionCount == 1) {
            Target target = collision.gameObject.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeTargetDamage(Gun.damage);
                particlesystem = fleshImpact;
            } else {
                particlesystem = impact;
            }
            
            Quaternion normalizedRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            ParticleSystem impactInstance = Instantiate(particlesystem, collision.contacts[0].point, normalizedRot);
            Destroy(impactInstance.gameObject, 3f);

            ParticleSystem bulletHoleInstance = Instantiate(bulletHole, collision.contacts[0].point, normalizedRot);
            bulletHoleInstance.transform.parent = collision.gameObject.transform;

            if (collision.gameObject.transform.GetComponent<Rigidbody>() != null) {
                collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(transform.forward * Gun.bulletForce, ForceMode.Impulse);
            }
        } else {
            Destroy(gameObject, 3f); 
        }
    }
}
