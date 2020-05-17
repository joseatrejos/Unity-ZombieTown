using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Win()
    {
        StartCoroutine(ExecuteAfterDelay());
    }

    public void RoundOver()
    {
        GameManager.instance.roundOver.SetActive(false);
    }

    IEnumerator ExecuteAfterDelay()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
}