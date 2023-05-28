using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static GameOverManager instance;


    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène");
            return;
        }

        instance = this;
    }

    public void IfPlayerDead(){
        gameOverUI.SetActive(true);
        
        Time.timeScale = 0;
    }

}