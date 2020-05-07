using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxDialogue : MonoBehaviour
{
    [SerializeField]
    Text dialogue;
    public string Dialogue { set => dialogue.text = value; }
    
    [SerializeField]
    string message ;
    public string Message { get => message; set => message = value; }

    [SerializeField, Range(0.001f, 4f)]
    float textSpeed = 0.025f;

    IEnumerator animateText;

    public void ShowDialogue()
    {
        animateText = Animate(textSpeed);
        StartCoroutine(animateText);
    }

    public void ClearText()
    {
        dialogue.text = "";
    }

    IEnumerator Animate(float time)
    {
        int i = 0;
        
        while(i < Message.Length)
        {
            dialogue.text += message[i];
            i++;
            yield return new WaitForSeconds(time);
        }
    }
}
