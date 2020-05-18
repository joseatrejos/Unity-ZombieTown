using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{        
    void QuitToMenu()
    {
        Application.Quit();
    }

    void TogglePause()
    {
        this.gameObject.SetActive(false);
        GameManager.instance.Unpause();
        Time.timeScale = 1;
    }
}
