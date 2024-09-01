using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour {

    public AudioMixer masterMixer;
    Slider musicSlider;
    Slider sfxSlider;

    void Start() {
        try {
            musicSlider = transform.parent.Find("MusicVolume").ConvertTo<Slider>();
            sfxSlider = transform.parent.Find("SFXVolume").ConvertTo<Slider>();
        } catch {
            return;
        }

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