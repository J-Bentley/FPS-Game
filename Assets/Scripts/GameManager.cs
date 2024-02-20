using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu;

    void Update() {
        if (!gamePaused && Input.GetKeyDown(KeyCode.Escape)) {
            if(Shop.isShopping) {
                shopMenu.SetActive(false);
            }
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
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0); 
    }

    public void OptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CreditsMenu() {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void BackButton() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
}
