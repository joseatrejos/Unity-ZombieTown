using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField]
    TextBoxDialogue textBox;

    [SerializeField, TextArea(3, 5)]
    string message;

    [SerializeField]
    int ScoreCost;

    string defaultMessage;

    bool canInteract = true;

    public bool CanInteract { get => canInteract; set => canInteract = value; }

    void Awake()
    {
        defaultMessage = message;
    }

    public void Unlock()
    {
        if (Input.GetButton("Accept"))
        {
            if (GameManager.instance.Score >= ScoreCost)
            {
                Destroy(gameObject, 1.3f);
                HideMessage();
                message = "Camino desbloqueado";
                ShowMessage();
                StartCoroutine(waitForHideMessage());
                GameManager.instance.Score -= ScoreCost;
                GameManager.instance.TxtScore.text = $"{GameManager.instance.Score}";
            }
            else
            {
                HideMessage();
                message = "Puntos Insuficientes";
                ShowMessage();
            }
        }
        else
        if (Input.GetButton("Cancel"))
        {
            HideMessage();
        }
    }

    public void ShowMessage()
    {
        textBox.gameObject.SetActive(true);
        textBox.Message = message;
        textBox.ShowDialogue();
    }

    public void HideMessage()
    {
        textBox.gameObject.SetActive(false);
        textBox.ClearText();
        message = defaultMessage;
    }

    public IEnumerator waitForHideMessage()
    {
        yield return new WaitForSeconds(1.0f);
        HideMessage();
    }

}
