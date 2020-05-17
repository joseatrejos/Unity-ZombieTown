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
        StartCoroutine(ExecuteAfterDelay("win"));
    }

    public void RoundOver()
    {
        StartCoroutine(ExecuteAfterDelay("roundOver"));
    }

    IEnumerator ExecuteAfterDelay(string gameEvent)
    {
        yield return new WaitForSeconds(4);
        switch(gameEvent)
        {
            case "win":
                SceneManager.LoadScene(0);
                break;

            case "roundOver":
                GameManager.instance.ChangeRound();
                break;
        }
    }
}