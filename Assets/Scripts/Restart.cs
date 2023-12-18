using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

    public void restartGame() {
        SceneManager.LoadScene(1);    
    }
}
