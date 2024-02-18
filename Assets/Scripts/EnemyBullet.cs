using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    [SerializeField] private int damage;
    [SerializeField] ParticleSystem impact;
    [SerializeField] ParticleSystem fleshImpact;
    [SerializeField] ParticleSystem bulletHole;
    private ParticleSystem impactParticlesystem;
    private int collisionCount;

    void OnCollisionEnter(Collision collision) {
        collisionCount++;
        if (collisionCount == 1) {
            Quaternion normalizedRot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            if (collision.transform.tag == "Player") {
                SpawnPlayer.playerInstance.GetComponent<Player>().TakeDamage(damage);
                impactParticlesystem = fleshImpact;
            } else {
                impactParticlesystem = impact;
                ParticleSystem bulletHoleInstance = Instantiate(bulletHole, collision.contacts[0].point, normalizedRot);
                bulletHoleInstance.transform.parent = collision.gameObject.transform; 
            }
            Instantiate(impactParticlesystem, collision.contacts[0].point, normalizedRot);
            Destroy(gameObject, 1f);
        }
    }
}
