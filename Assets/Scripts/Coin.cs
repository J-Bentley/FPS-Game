using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour {
    [SerializeField] private int minAmount = 10;
    [SerializeField] private int maxAmount = 30;
    [SerializeField] private TextMeshProUGUI moneyText;
    private AudioSource[] audioSource;
    [SerializeField] private CharacterController playerController;

    void Start() {
        audioSource = playerController.GetComponents<AudioSource>();
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.transform.tag == "Player") {
            int randomAmount = Random.Range(minAmount, maxAmount);
            Player.money += randomAmount;
            moneyText.text = "$" + Player.money;
            audioSource[7].Play(); //coin pickup sound
            Destroy(transform.gameObject);
        }
    }

}
