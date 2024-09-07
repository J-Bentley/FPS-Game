using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    AudioSource backgroundMusic;

    void Start() {
        backgroundMusic = GetComponent<AudioSource>();
        
    }

    void Update() {
        if (!backgroundMusic.isPlaying) {
            backgroundMusic.Play();
        }
    }
}
