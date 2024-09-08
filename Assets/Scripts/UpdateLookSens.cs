using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateLookSens : MonoBehaviour {

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI SensitivityText;

    void Start() {
        sensitivitySlider.value = MouseLook.lookSensitivity;
        SensitivityText.text = MouseLook.lookSensitivity.ToString();
    }

    public void UpdateLookSensitivity() {
        MouseLook.lookSensitivity = sensitivitySlider.value;
        SensitivityText.text = MouseLook.lookSensitivity.ToString();
    }
}
