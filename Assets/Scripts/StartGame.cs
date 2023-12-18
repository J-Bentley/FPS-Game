using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    
    public void beginGame() {
        SceneManager.LoadScene(1);    
    }
}
