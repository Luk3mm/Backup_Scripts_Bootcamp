using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovimentTest : MonoBehaviour
{
    [Header("Movement Components")]
    public float speed;
    public float gyrus;
    public float dash;
    public float heightJump;
    public bool canMove = true;

    private float initialSpeed;
    private bool isGround;
    private float yForce;

    [Header("Components")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask collisionLayer;
    [SerializeField]
    private Animator door;

    private CharacterController playerController;
    private Transform myCamera;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        initialSpeed = speed;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        Moviment();
        Dash();
        Jump();
    }

    public void Moviment()
    {
        if (!canMove)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moviment = new Vector3(horizontal, 0, vertical);

        moviment = myCamera.TransformDirection(moviment);
        moviment.y = 0;

        playerController.Move(moviment * Time.deltaTime * speed);

        if (moviment != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moviment), Time.deltaTime * gyrus);
        }

        animator.SetBool("move", moviment != Vector3.zero);

        isGround = Physics.CheckSphere(groundCheck.position, 0.1f, collisionLayer);
        animator.SetBool("isground", isGround);
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = dash;
            animator.SetBool("dash", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = initialSpeed;
            animator.SetBool("dash", false);
        }
    }

    public void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            yForce = heightJump;
            animator.SetTrigger("jump");
        }

        if(yForce > -9.81f)
        {
            yForce += -9.81f * Time.deltaTime; 
        }

        playerController.Move(new Vector3(0, yForce, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            door.SetTrigger("open");
        }
    }
}
