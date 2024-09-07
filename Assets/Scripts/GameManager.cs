using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public static bool gamePaused = false;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject gameOptionsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject mainMenu;

    void Update() {
        EscapeHandling();
    }

    private void EscapeHandling() {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (sceneIndex == 0) {
                QuitGame();
            }
            else if (sceneIndex == 1) {
                if (!gamePaused) {
                    PauseGame();
                }
                else if (gamePaused) {
                    ResumeGame();
                }
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
        gameOptionsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame() {
        SceneManager.LoadScene(1);  
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit.");
    }

    public void Menu() { //go back to main menu from ingame
        Time.timeScale = 1f;
        gamePaused = false;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0); 
    }

    public void OptionsMenu() { //main menu options
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void GameOptionsMenu() { //go to in game options from pause screen
        pauseMenu.SetActive(false);
        gameOptionsMenu.SetActive(true);
    }

    public void CreditsMenu() {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void GameBackButton() { //go back to pause screen from in game options
        pauseMenu.SetActive(true);
        gameOptionsMenu.SetActive(false);
    }

    public void BackButton() { //back to main menu from options/credits at main menu
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
}
