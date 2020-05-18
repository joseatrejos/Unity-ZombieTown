using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        //SceneManager.LoadScene("MainMenu");
    }

    public void RoundOver()
    {
        GameManager.instance.roundOver.SetActive(false);
    }
}