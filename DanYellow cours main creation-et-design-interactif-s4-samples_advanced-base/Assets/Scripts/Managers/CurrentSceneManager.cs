using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    public StringEventChannelSO OnLevelEnded;
    public VoidEventChannelSO onRestartLastCheckpoint;

    public BoolEventChannelSO onTogglePauseEvent;
    public GameObject pauseMenuUI;

    public GameObject gameOverUI;

    public GameObject SettingsWindow;

    bool isGamePaused = false;


    private void Awake() {
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);

    }

    private void OnEnable() {
        // on souscrit aux evenements
        OnLevelEnded.OnEventRaised +=  LoadScene;
    }

    private void OnDisable() {
        // On se desabonne aux evenements
        OnLevelEnded.OnEventRaised +=  LoadScene;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            RestartLastCheckpoint();
            

        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleGameWindowSizeInEditor();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            QuitGame();
        }

        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            LoadScene("Debug");
        }
#endif
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
        pauseMenuUI.SetActive(isGamePaused);
    }

    public void RestartLastCheckpoint()
    {
        Debug.Log("RestartLastCheckpoint");
        onRestartLastCheckpoint.Raise();
        Time.timeScale = 1;
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
        pauseMenuUI.SetActive(isGamePaused);
        // Refill life to full
        // Position to last checkpoint
        // Remove menu
        // Reset Rigidbody
        // Reactivate Player movements
        // Reset Player's rotation
    }

    public void GameOverScreen()
    {
        Debug.Log("GameOverScreen");

    }

    public void SettingsButton(){
        SettingsWindow.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        SettingsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    #if UNITY_EDITOR
    private void ToggleGameWindowSizeInEditor()
    {
        UnityEditor.EditorWindow window = UnityEditor.EditorWindow.focusedWindow;
        window.maximized = !window.maximized;
    }
    #endif
}
