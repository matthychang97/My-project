using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    public float movementSpeed = 5f;
    public float JumpHeight = 2f;

    public float fallGravityMultiplier = 2f;
    private float forwardInputValue;
    private float strafeInputValue;
    private bool jumpInput;

    //Physics Fall velocity
    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private CharacterController characterController;

    void Awake ()
    { 
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        Movement();
        JumpAndGravity();
    }

    void Movement()
    {
        Vector3 direction = (transform.forward * forwardInputValue + transform.right * strafeInputValue).normalized * movementSpeed * Time.deltaTime;

        //Add physics using Vector3s up direction (World coordinates) as the direction of gravity
        direction += Vector3.up * verticalVelocity * Time.deltaTime;

        characterController.Move(direction);
    }

    void JumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            //stop velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if(jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            //apply gravity over time if under terminal gravity
            if(verticalVelocity < terminalVelocity)
            {
                //Set gravity multiplier if falling downwards
                float gravityMultiplier = 1;
                if(characterController.velocity.y < -1)
                {
                    gravityMultiplier = fallGravityMultiplier;
                }
                verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            }
        }
    }
}
