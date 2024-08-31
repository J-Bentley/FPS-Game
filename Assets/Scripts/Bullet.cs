using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float headshotMultiplier;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    [SerializeField] ParticleSystem fleshBulletHole;
    ParticleSystem impactParticlesystem;
    ParticleSystem bulletHoleParticlesystem;
    float collisionCount;
    Target target;
    
    void OnCollisionEnter(Collision collision) {
        collisionCount++;
        if (collisionCount == 1) {
            try {
                target = collision.gameObject.transform.parent.GetComponent<Target>();
            } catch {
                target = collision.gameObject.transform.GetComponent<Target>();
            }
            if (target != null) {
                if (collision.gameObject.transform.tag == "Head") {
                    target.TakeTargetDamage(Gun.damage * headshotMultiplier);
                } else {
                    target.TakeTargetDamage(Gun.damage);
                }
                impactParticlesystem = fleshImpact;
                bulletHoleParticlesystem = fleshBulletHole;
            } else {
                impactParticlesystem = impact;
                bulletHoleParticlesystem = bulletHole;
            }

            Quaternion normalizedRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);

            ParticleSystem impactInstance = Instantiate(impactParticlesystem, collision.contacts[0].point, normalizedRot);
            impactInstance.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);

            ParticleSystem bulletHoleInstance = Instantiate(bulletHoleParticlesystem, collision.contacts[0].point, normalizedRot);
            bulletHoleInstance.transform.parent = collision.gameObject.transform;

            Destroy(gameObject, 1f); 
        }
    }
}