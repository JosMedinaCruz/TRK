using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    public bool isKnocking;
    public float knockBackLenght = 0.5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    //References
    public CharacterController charController;
    private Animator animator;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); //Player / Modelo - Animator
    }

    private void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }   

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); //true cuando esta en ground

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);
        //moveDirection *= walkSpeed;
        if(isGrounded)
        {
                if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)) //Sino se mueve y sino se presiona shift
                {
                    //Walk
                    Walk();
                }
                else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) //sion se mueve y se presiona shift
                {
                    //Run
                    Run();
                }
                else if(moveDirection == Vector3.zero)
                {
                    //Idle
                    Idle();
                }
                
            moveDirection *= moveSpeed;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
       
        charController.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime; //Gravedad al personaje

        charController.Move(velocity * Time.deltaTime); // Se le aplica la gravedad
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1f);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private IEnumerator Attack()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);//Cuando ataca
        animator.SetTrigger("Attack");//Animacion

        yield return new WaitForSeconds(0.9f); //Espera 1 s antes de hacer algo
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
    }
}
