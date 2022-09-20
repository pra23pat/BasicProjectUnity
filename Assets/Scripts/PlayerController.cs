using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Player playerInput;
    Animator anim;

    private void Awake()
    {
        playerInput = new Player();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float rotationSpeed = 4f;

    private Transform child;
    private Transform cameraMain;

    private void Start()
    {
        cameraMain = Camera.main.transform;
        child = transform.GetChild(0).transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        
        


        Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();

        //(movementInput.x >=1 && movementInput.y >=1) || (movementInput.x <= -1 && movementInput.y >= 1) || (movementInput.x <= -1 && movementInput.y <= -1) || (movementInput.x >= 1 && movementInput.y <= -1)

        /*/if (((movementInput.x >= 0.5 && movementInput.y >= 0.5) || (movementInput.x <= -0.5 && movementInput.y >= 0.5) || (movementInput.x <= -0.5 && movementInput.y <= -0.5) || (movementInput.x >= 0.5 && movementInput.y <= -0.5)))
        {
            anim.SetBool("Run", true);
        }
        if (!(movementInput.x >= 0.5 && movementInput.y >= 0.5) || (movementInput.x <= -0.5 && movementInput.y >= 0.5) || (movementInput.x <= -0.5 && movementInput.y <= -0.5) || (movementInput.x >= 0.5 && movementInput.y <= -0.5)) 
        {
            anim.SetBool("Run", false);
        }*/
        if(movementInput.x != 0f && movementInput.y != 0f )
        {
            anim.SetBool("Run", true);
        }
        if (!(movementInput.x != 0f && movementInput.y != 0f))
        {
            anim.SetBool("Run", false);
        }

        Vector3 move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);



         //Changes the height position of the player..
         if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
         {
            anim.SetTrigger("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -0.8f * gravityValue);
         }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (movementInput != Vector2.zero)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(child.localEulerAngles.x, cameraMain.localEulerAngles.y, child.localEulerAngles.z));
            child.rotation = Quaternion.Lerp(child.rotation, rotation, Time.deltaTime * rotationSpeed);
        }


    }

}
