using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    public float dialogRange;
    public LayerMask playerLayer;

    public DialogueSettings dialogue;

    bool playerHit;

    private List<string> sentences = new List<string>();
    private List<string> actorName = new List<string>();
    private List<Sprite> actorSprite = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetNPCInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHit)
        {
            DialogControlTest.instance.Speech(sentences.ToArray(), actorName.ToArray(), actorSprite.ToArray());
        }
    }

    private void FixedUpdate()
    {
        ShowDialog();
    }

    void GetNPCInfo()
    {
        for (int i = 0; i < dialogue.dialogues.Count; i++)
        {
            switch (DialogControlTest.instance.language)
            {
                case DialogControlTest.idiom.pt:
                    sentences.Add(dialogue.dialogues[i].sentence.portuguese);
                    break;

                case DialogControlTest.idiom.eng:
                    sentences.Add(dialogue.dialogues[i].sentence.english);
                    break;

                case DialogControlTest.idiom.spa:
                    sentences.Add(dialogue.dialogues[i].sentence.spanish);
                    break;
            }

            actorName.Add(dialogue.dialogues[i].actorName);
            actorSprite.Add(dialogue.dialogues[i].profile);
        }
    }

    void ShowDialog()
    {
        Vector3 point1 = transform.position + Vector3.up * dialogRange;
        Vector3 point2 = transform.position - Vector3.up * dialogRange;

        Collider[] hits = Physics.OverlapCapsule(point1, point2, dialogRange, playerLayer);

        if (hits.Length > 0)
        {
            playerHit = true;
        }
        else
        {
            playerHit = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dialogRange);
    }
}
