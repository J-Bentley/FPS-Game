using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {

    public AudioMixer masterMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start() {
        masterMixer.GetFloat("musicVol", out float musicVolume);
        musicSlider.value = musicVolume;

        masterMixer.GetFloat("sfxVol", out float sfxVolume);
        sfxSlider.value = sfxVolume;
    }

    public void SetMusicVolume(float soundLevel) {
        masterMixer.SetFloat ("musicVol", soundLevel);
    }

    public void SetSfxVolume(float soundLevel) {
        masterMixer.SetFloat ("sfxVol", soundLevel);
    }
}