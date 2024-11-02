using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
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

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        Move();
        Look();
        ApplyGravity();
    }

    private void Move()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (isCollisionEnabled)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), true);
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Default"), false);
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

        if (isCollisionEnabled || characterController.isGrounded)
        {
            characterController.Move(velocity * Time.deltaTime);
        }
    }
    private void ToggleCollision()
    {
        isCollisionEnabled = !isCollisionEnabled;
    }
}
