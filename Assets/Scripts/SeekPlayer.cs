using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour {

    NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        agent.SetDestination(SpawnPlayer.playerInstance.transform.position);
    }
}
