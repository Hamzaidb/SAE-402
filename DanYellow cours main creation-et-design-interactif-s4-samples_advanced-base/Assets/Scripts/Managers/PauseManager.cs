using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseManager : MonoBehaviour
{
    public BoolEventChannelSO onTogglePauseEvent;
    public GameObject pauseMenuUI;
   


    bool isGamePaused = false;


    private void Awake() {
        pauseMenuUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            
        }
    }




   
    public void Resume()
    {
        Time.timeScale = 1;
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
        pauseMenuUI.SetActive(isGamePaused);
    }


    void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        onTogglePauseEvent.Raise(isGamePaused);
        pauseMenuUI.SetActive(isGamePaused);
    }

    // public bool IsGamePaused()
    // {
    //     return isGamePaused;
    // }
}
