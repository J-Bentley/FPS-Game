
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;

    void Update() {
        if(!gamePaused && Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        } else if (gamePaused && Input.GetKeyDown(KeyCode.Escape)) {
            ResumeGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        gamePaused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame() {
        SceneManager.LoadScene(1);  
    }

    public void QuitGame() {
        Application.Quit(); 
    }

    public void Menu() {
        SceneManager.LoadScene(0); 
    }
}
