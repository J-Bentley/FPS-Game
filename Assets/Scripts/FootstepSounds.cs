using UnityEngine;

public class FootstepSounds : MonoBehaviour {

    [SerializeField] float walkInterval = 0.5f;
    [SerializeField] float sprintInterval = 0.2f;
    float originalWalkInterval;
    Transform player;
    RaycastHit raycast;
    AudioSource[] audioSources;
    int index;
    float timer;

    void Start() {
        player = Player.instance.transform;
        audioSources = GetComponents<AudioSource>();
        originalWalkInterval = walkInterval;
    }

    void Update() {
        bool isMoving = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            walkInterval = sprintInterval;
        } else {
            walkInterval = originalWalkInterval;
        }

        if (isMoving && Player.instance.isGrounded) {
            timer += Time.deltaTime;
            if (timer > walkInterval) {
                timer = 0;
                PlaySound();
            }
            GroundCast();
        } else {
            timer = 0;
        }
    }

    void GroundCast() {
        Physics.Raycast(player.transform.position, Vector3.down, out raycast);
        switch (raycast.transform.tag) {
            case "Concrete":
                index = 0;
                Debug.Log("Stepping on concrete");
                break;
            case "Grass":
                index = 1;
                Debug.Log("Stepping on grass");
                break;
            case "Wood":
                index = 2;
                Debug.Log("Stepping on wood");
                break;
        }
    }

    void PlaySound() {
        if (!audioSources[index].isPlaying) {
            audioSources[index].pitch = Random.Range(0.8f, 1.2f);
            audioSources[index].Play();
        }
    }
}
