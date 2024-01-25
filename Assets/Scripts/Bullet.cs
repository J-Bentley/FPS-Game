using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    [SerializeField] ParticleSystem fleshBulletHole;
    private ParticleSystem impactParticlesystem;
    private ParticleSystem bulletHoleParticlesystem;

    void OnCollisionEnter(Collision collision) {
        Target target = collision.gameObject.transform.GetComponent<Target>();
        if (target != null) {
            target.TakeTargetDamage(Gun.damage);
            impactParticlesystem = fleshImpact;
            bulletHoleParticlesystem = fleshBulletHole;
        } else {
            impactParticlesystem = impact;
            bulletHoleParticlesystem = bulletHole;
        }
            
        Quaternion normalizedRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);

        Instantiate(impactParticlesystem, collision.contacts[0].point, normalizedRot);

        ParticleSystem bulletHoleInstance = Instantiate(bulletHoleParticlesystem, collision.contacts[0].point, normalizedRot);
        bulletHoleInstance.transform.parent = collision.gameObject.transform;

        if (collision.gameObject.transform.GetComponent<Rigidbody>() != null) {
            collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(transform.forward * Gun.bulletForce, ForceMode.Impulse);
        }
        
        Destroy(gameObject); 
    }
}
