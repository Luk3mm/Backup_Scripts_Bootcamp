using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControlTest : MonoBehaviour
{
    [System.Serializable]
    public enum idiom
    {
        pt,
        eng,
        spa
    }

    public idiom language;

    [Header("Components")]
    public GameObject dialogObj;
    public Image profileSprite;
    public Text speechText;
    public Text actorNameText;

    [Header("Settings")]
    public float typingSpeed;

    //Control Varible
    public bool isShowing;
    private int index;
    private string[] sentences;
    private string[] actorNames;
    private Sprite[] actorProfiles;

    //private PlayerMovimentTest player;

    public static DialogControlTest instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //player = FindObjectOfType<PlayerMovimentTest>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextSentence();
        }
    }

    IEnumerator TypeSentence()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if (speechText.text == sentences[index])
        {
            if (index < sentences.Length - 1)
            {
                index++;
                profileSprite.sprite = actorProfiles[index];
                actorNameText.text = actorNames[index];
                speechText.text = "";
                StartCoroutine(TypeSentence());
            }
            else
            {
                speechText.text = "";
                actorNameText.text = "";
                index = 0;
                dialogObj.SetActive(false);
                sentences = null;
                isShowing = false;
                //player.isPaused = false;
            }
        }
    }

    public void Speech(string[] txt, string[] actorName, Sprite[] actorProfile)
    {
        if (!isShowing)
        {
            dialogObj.SetActive(true);
            sentences = txt;
            actorNames = actorName;
            actorProfiles = actorProfile;
            profileSprite.sprite = actorProfiles[index];
            actorNameText.text = actorNames[index];
            StartCoroutine(TypeSentence());
            isShowing = true;
            //player.isPaused = true;
        }
    }
}
