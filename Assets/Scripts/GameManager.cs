using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject gameOptionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject mainMenu;

    void Update() {
        if (!gamePaused && Input.GetKeyDown(KeyCode.Escape)) {
            try {
                PauseGame();
            } catch {
                QuitGame();
            }
        } else if (gamePaused && Input.GetKeyDown(KeyCode.Escape)) {
            try {
                ResumeGame();
            } catch {
                QuitGame();
            }
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        gamePaused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame() {
        SceneManager.LoadScene(1);  
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit.");
    }

    public void Menu() {
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0); 
    }

    public void OptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void GameOptionsMenu() {
        pauseMenu.SetActive(false);
        gameOptionsMenu.SetActive(true);
    }

    public void CreditsMenu() {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void GameBackButton() {
        pauseMenu.SetActive(true);
        gameOptionsMenu.SetActive(false);
    }

    public void BackButton() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
}
