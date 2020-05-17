using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField]
    TextBoxDialogue textBoxDialogue;

    [SerializeField, TextArea(3, 5)]
    string npcDialogue;

    public void StartTalking()
    {
        textBoxDialogue.gameObject.SetActive(true);
        textBoxDialogue.Message = npcDialogue;
        textBoxDialogue.ShowDialogue();
    }

    public void StopTalking()
    {
        textBoxDialogue.gameObject.SetActive(false);
        textBoxDialogue.ClearText();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StartTalking();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            StopTalking();
        }
    }
}
