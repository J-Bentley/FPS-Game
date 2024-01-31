using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] float headshotMultiplier;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    [SerializeField] ParticleSystem fleshBulletHole;
    private ParticleSystem impactParticlesystem;
    private ParticleSystem bulletHoleParticlesystem;
    private float collisionCount;
    


    void OnCollisionEnter(Collision collision) {
        collisionCount++;
        if (collisionCount == 1) {
            Target target = collision.gameObject.transform.parent.GetComponent<Target>();
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
            Instantiate(impactParticlesystem, collision.contacts[0].point, normalizedRot);
            ParticleSystem bulletHoleInstance = Instantiate(bulletHoleParticlesystem, collision.contacts[0].point, normalizedRot);
            bulletHoleInstance.transform.parent = collision.gameObject.transform; 
            Destroy(gameObject, 1f); 
        }
    }
}
