using UnityEngine;
using TMPro;

public class Door : MonoBehaviour {
    
    [SerializeField] private float interactRange;
    [SerializeField] private LayerMask doorLayer;
    [SerializeField] private int cost;
    [SerializeField] TextMeshProUGUI equipText;
    private Camera cam;
    private AudioSource[] audioSources;
    private RaycastHit hit;
    private GameObject doorObject;
    //[SerializeField] private Player playerScript;

    void Start() {
        cam = Camera.main;
        audioSources = GetComponents<AudioSource>();
    }

    void Update() {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, doorLayer)) {
            equipText.enabled = true;
            equipText.text = "[E] Open for $" + cost;
            if (Input.GetKeyDown(KeyCode.E)) {
                doorObject = hit.transform.gameObject;
                if (Player.wallet >= cost) {
                    Player.wallet -= cost;
                    audioSources[8].Play();
                    doorObject.SetActive(false);
                } else {
                    audioSources[9].Play();
                }
            }
        }
    }
}

