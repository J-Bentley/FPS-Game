using UnityEngine;
using TMPro;

public class GameOverStats : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Start() {
        waveText.text = "Waves Survived: " + SpawnEnemies.wave;
        enemiesKilledText.text = "Enemies Killed: " + SpawnEnemies.totalEnemiesKilled;
        moneyText.text = "Total Money: $" + Player.totalMoney;
    }
}
