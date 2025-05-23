using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    [Header("Components")]
    public GameObject dialogueObj;
    public Text actorNameText;
    public Text speechText;

    [Header("Settings")]
    public float typingSpeed;
    private string[] sentences;
    private int index;
    private Coroutine typingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextSSentence();
        }
    }

    public void Speech(string[] txt, string actorName)
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueObj.SetActive(true);
        speechText.text = "";
        actorNameText.text = actorName;
        sentences = txt;
        index = 0;
        typingCoroutine = StartCoroutine(TypingSentence());
    }

    IEnumerator TypingSentence()
    {
        speechText.text = "";

        foreach(char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
     }

    public void NextSSentence()
    {
        if(speechText.text == sentences[index])
        {
            if(index < sentences.Length - 1)
            {
                index++;
                speechText.text = "";
                
                if(typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypingSentence());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void HidePanel()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        speechText.text = "";
        actorNameText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }

    public void EndDialogue()
    {
        speechText.text = "";
        index = 0;
        dialogueObj.SetActive(false);
    }
}
