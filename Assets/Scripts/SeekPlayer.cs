using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour {

    private NavMeshAgent agent;
    public Player playerScript;
    public CharacterController playerController;
    public float attackDamage = 10f;
    

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        agent.SetDestination(playerController.transform.position);
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.transform.tag == "Player"){
            playerScript.TakeDamage(attackDamage);
        }
    }

    void OnTriggerStay(Collider collider){
        if(collider.gameObject.transform.tag == "Player"){
            playerScript.TakeDamage(0.1f);
        }
    }
}
