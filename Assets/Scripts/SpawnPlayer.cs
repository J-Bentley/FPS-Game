using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

    [SerializeField] private GameObject player;
    public static GameObject playerInstance;
    
    void Start() {
        playerInstance = Instantiate(player, gameObject.transform.position, Quaternion.identity);
    }
    void Update() {
        //Debug.Log(playerInstance.transform.position.x);
    }
}
