using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour {

    private NavMeshAgent agent;
    public CharacterController playerController;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        agent.SetDestination(playerController.transform.position);
    }
}
