using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField]
    TextBoxDialogue textBox;

    [SerializeField,TextArea(3,5)]
    string message;

    [SerializeField]
    int ScoreCost;
    // Start is called before the first frame update

    string defaultMessage;
    void Start()
    {
        
    }

     void Awake()
    {
        defaultMessage = message;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
            if(Input.GetButton("Acept"))
            {
                if(GameManager.instance.Score >= ScoreCost)
                {
                    Destroy(gameObject,1.3f);
                    HideMessage();
                    message = "Camino desbloqueado";
                    ShowMessage();
                    StartCoroutine(waitForHideMessage());
                    
                }else
                {
                    HideMessage();
                    message = "Puntos Insuficientes";
                    ShowMessage();
                }
            }else
            if(Input.GetButton("Cancel"))
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
