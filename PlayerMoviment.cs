using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoviment : MonoBehaviour
{
    [Header("Movement variable")]
    public float speed;
    public float gyrus;
    public float dash;
    public float heightJump;

    private float initialSpeed;
    private bool isGround;
    private float yForce;

    [Header("Components")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask collisionLayer;

    private CharacterController playerController;
    private Transform myCamera;
    private Animator animator;
    private PlayerInput inputs;

    private InputAction jump;
    private InputAction dashAction;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        inputs = GetComponent<PlayerInput>();
        initialSpeed = speed;

        dashAction = inputs.actions["Dash"];
        jump = inputs.actions["Jump"];
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moviment = new Vector3(horizontal, 0, vertical);

        moviment = myCamera.TransformDirection(moviment);
        moviment.y = 0;

        playerController.Move(moviment * Time.deltaTime * speed);
        
        if(moviment != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moviment), Time.deltaTime * gyrus);
        }

        animator.SetBool("move", moviment != Vector3.zero);

        isGround = Physics.CheckSphere(groundCheck.position, 0.3f, collisionLayer);
        animator.SetBool("isground", isGround);
    }

    public void Dash()
    {
        bool isDash = dashAction.ReadValue<float>() > 0.5f;

        if (isDash)
        {
            speed = dash;
            animator.SetBool("dash", true);
        }
        else
        {
            speed = initialSpeed;
            animator.SetBool("dash", false);
        }
    }

    public void Jump()
    {
        if(jump.triggered && isGround)
        {
            Debug.Log(Gamepad.current);
            yForce = heightJump;
            animator.SetTrigger("jump");
        }

        if(yForce > -9.81f)
        {
            yForce += -9.81f * Time.deltaTime;
        }

        playerController.Move(new Vector3(0, yForce, 0) * Time.deltaTime);
    }
}
