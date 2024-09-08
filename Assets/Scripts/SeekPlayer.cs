using UnityEngine;
using UnityEngine.AI;

public class SeekPlayer : MonoBehaviour {

    NavMeshAgent agent;
    public GameObject playerObject;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        playerObject = Player.instance.gameObject;
    }

    void Update() {
        agent.SetDestination(playerObject.transform.position);
    }
}
