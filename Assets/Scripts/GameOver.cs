using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Salir");
        //SceneManager.LoadScene(menu);
    }

    public void Win()
    {
        Debug.Log("You win carnal");
        //SceneManager.LoadScene(menu);
    }
}
