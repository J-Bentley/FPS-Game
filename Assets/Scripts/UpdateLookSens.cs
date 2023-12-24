using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateLookSens : MonoBehaviour {

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI LookSensText;

    void Start() {
        slider.value = MouseLook.lookSensitivity;
        LookSensText.text = MouseLook.lookSensitivity.ToString();
    }

    public void UpdateLookSensitivity() {
        MouseLook.lookSensitivity = slider.value;
        LookSensText.text = MouseLook.lookSensitivity.ToString();
    }
}
