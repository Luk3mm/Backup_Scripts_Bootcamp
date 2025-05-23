using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("Components")]
    public string[] speechText;
    public string actorName;
    public float radius;

    private bool onRadius;
    private bool isDialogActive = false;

    public LayerMask playerLayer;
    private DialogControl dControl;

    // Start is called before the first frame update
    void Start()
    {
        dControl = FindObjectOfType<DialogControl>();
    }

    private void FixedUpdate()
    {
        Interact();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && onRadius && !isDialogActive)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogActive = true;
        dControl.Speech(speechText, actorName);
        Debug.Log("Dialogo Iniciado com " + actorName);
    }

    private void EndDialogue()
    {
        isDialogActive = false;
        dControl.HidePanel();
        Debug.Log("Dialogo Encerrado!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void Interact()
    {
        Vector3 point1 = transform.position + Vector3.up * radius;
        Vector3 point2 = transform.position - Vector3.up * radius;

        Collider[] hits = Physics.OverlapCapsule(point1, point2, radius, playerLayer);

        //Collider2D hit = Physics2D.OverlapCircle(transform.position, radious, playerLayer); //Essas duas linhas sao para o 2D e substitui as de cima
        //if (hit != null)

        if (hits.Length > 0)
        {
            if (!onRadius)
            {
                Debug.Log("Player entrou no raio de interacao");
            }

            onRadius = true;
        }
        else
        {
            if (onRadius)
            {
                Debug.Log("Player saiu do raio de interacao");
            }

            onRadius = false;

            if (isDialogActive)
            {
                EndDialogue();
            }
        }
    }
}
