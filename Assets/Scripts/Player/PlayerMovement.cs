using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
    //movement
    public float movementSpeed = 5f;
    public float JumpHeight = 2f;
    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;
    private float forwardInputValue;
    private float strafeInputValue;
    private bool jumpInput;

    //Physics Fall velocity
    private float terminalVelocity = 53f;
    private float verticalVelocity;

    //Mouse movement
    private float rotateCameraPitch;
    private Camera firstPersonCam;
    private CharacterController characterController;

    //Zoom
    public float defaultFOV = 60f;
    public float zoomFOV = 30f;
    public float zoomSpeed = 8f; //higher or lower = transition speed
    private bool zoomInput;
    private float targetFOV;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Awake ()
    { 
        characterController = GetComponent<CharacterController>();
        firstPersonCam = GetComponentInChildren<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;

        defaultFOV = firstPersonCam.fieldOfView;
        targetFOV = defaultFOV;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnEnable()
    {
        // CharacterController resists direct transform changes unless briefly disabled
        characterController.enabled = false;
        transform.position = startPosition;
        transform.rotation = startRotation;
        characterController.enabled = true;

        verticalVelocity = 0f;
        rotateCameraPitch = 0f;
        firstPersonCam.fieldOfView = defaultFOV;
        targetFOV = defaultFOV;
    }
    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        zoomInput = Input.GetButton("Fire2"); //right mouse button

        Movement();
        JumpAndGravity();
        CameraMovement();
        Zoom();
    }
    void CameraMovement()
    {
        //Rotate player
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        //Up down for Camera
        rotateCameraPitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        //Lock rotation so we can't flip
        rotateCameraPitch = Mathf.Clamp(rotateCameraPitch, -pitchRange, pitchRange);
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotateCameraPitch, 0, 0);
    }

    void Zoom()
    {
        targetFOV = zoomInput ? zoomFOV : defaultFOV;
        firstPersonCam.fieldOfView = Mathf.Lerp(firstPersonCam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
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
