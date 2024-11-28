using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask collisionMask;

    private Movement controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Transform cameraTransform;
    private CharacterController characterController;
    private Vector3 velocity;

    private bool isCollisionEnabled = true;
    private float xRotation = 0f;

    //Needed for sound while moving
    public GameObject audioControllerObject;
    bool playOnce = false;
    private AudioController audioController;


    private void Awake()
    {
        controls = new Movement();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Player.Toggle.performed += _ => ToggleCollision();

        cameraTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }

    void Start(){
        audioController = audioControllerObject.GetComponent<AudioController>();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        Move();
        Look();
        ApplyGravity();
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject);
        // If collide with a wall
        if(other.gameObject.layer == 3) {
            Debug.Log("Collision with Wall Layer Detected");
            
            audioController.PlayPlayerHitWall();
        }
    }
    private void Move()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        bool hasMovementInput = (moveInput != Vector2.zero);

        if (hasMovementInput && !playOnce)
        {
            playOnce = true;
            // Start playing the running sound
            audioController.PlayRunning();
        }
        else if(!hasMovementInput)
        {
            // Stop playing the running sound
            audioController.StopRunning();
            playOnce = false;
        }
    }

    private void Look()
    {
        float mouseX = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = lookInput.y * lookSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void ToggleCollision()
    {
        isCollisionEnabled = !isCollisionEnabled;
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), !isCollisionEnabled);
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Wall"), !isCollisionEnabled);
    }
}
