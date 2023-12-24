using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetMusicVolume(float soundLevel) {
        masterMixer.SetFloat ("musicVol", soundLevel);
    }

    public void SetSfxVolume(float soundLevel) {
        masterMixer.SetFloat ("sfxVol", soundLevel);
    }
}