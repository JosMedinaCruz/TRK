using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;

    public float rotateSpeed = 5f;

    private Vector3 moveDirection;

    public CharacterController charController;
    public Camera playerCamera;
    public GameObject playerModel;

    public Animator animator;

    public bool isKnocking;
    public float knockBackLenght = 0.5f;
    private float knockBackCounter;
    public Vector2 knockBackPower;

    public int playerSound;

    public GameObject[] playerPieces;

    public float bounceForce = 8f;

    public bool stopMove; //Cuando obtiene la gema

     public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main; //MainCamera de la escena
    }

    // Update is called once per frame
    void Update()
    {
        if(!isKnocking && !stopMove)
        {
            float yStore = moveDirection.y;
            //Movimiento del personaje
            //moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));  //forward -->
            moveDirection.Normalize();
            moveDirection= moveDirection * moveSpeed;
            moveDirection.y = yStore; // Almacena el valor de y 

                //Salto
                if(charController.isGrounded)
                {
                    moveDirection.y = 0f; /// < ---- ojito
                    if(Input.GetButtonDown("Jump"))
                    {
                        moveDirection.y = jumpForce;
                    }
                }
                
                moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale; // Gravedad

                charController.Move(moveDirection * Time.deltaTime);
        
                if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)  //Sino se mueve el personaje       
                {
                    transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f); //Rotacion Camara 
                    Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z)); //El jugador rota con el eje deseado
                    playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);//Movimiento suave
                }
        }

            if(isKnocking)
            {
                knockBackCounter -= Time.deltaTime;

                float yStore = moveDirection.y;
                moveDirection = (playerModel.transform.forward * knockBackPower.x);  //forward -->
                moveDirection.y = yStore; // Almacena el valor de y 

                moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale; // Gravedad

                charController.Move(moveDirection * Time.deltaTime);

                if(knockBackCounter <= 0)
                {
                    isKnocking = false;
                }
            }
            
            if(stopMove)
            {
                moveDirection = Vector3.zero;
                 moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
                charController.Move(moveDirection);
            }

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Attack();
                StartCoroutine(Attack());
                //AudioManager.instance.PlaySFX(playerSound);
                Debug.Log("attack");
            }

            animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z)); 
            animator.SetBool("Grounded", charController.isGrounded);
        
    }

    //Función knockback
    public void Knockback()
    {
        isKnocking =  true;
        knockBackCounter = knockBackLenght;
        Debug.Log("Knocked back");
        moveDirection.y = knockBackPower.y; //en el eje y
        charController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        charController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
         
        //animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);//Cuando ataca
        animator.SetTrigger("Attack");//Animacion

        yield return new WaitForSeconds(0.9f); //Espera 1 s antes de hacer algo
        //animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
    }
}
