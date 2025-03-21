using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject pausePanel;

    private bool isGamePause = false;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            isGamePause = !isGamePause;
            PauseGame();
        }
        
    }

    public void PauseGame(){
        if(isGamePause){
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else{
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

    }
}
