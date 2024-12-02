using UnityEngine;
public class Return : MonoBehaviour
{
    private Movement controls;
    private CharacterController characterController;
    private Rigidbody rb;
    private Vector3 startPosition = new Vector3(-2f, 0, 0);

    private void Awake()
    {
        controls = new Movement();
        controls.Player.Return.performed += _ => TeleportToOrigin();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    public void TeleportToOrigin()
    {
        Debug.Log("Teleporting");
        if (characterController != null)
        {
            characterController.enabled = false;
            // transform.position = Vector3.zero;

            transform.position = startPosition;
            characterController.enabled = true;
        }
        else if (rb != null)
        {
            // rb.MovePosition(Vector3.zero);
            rb.MovePosition(startPosition);
        }
        else
        {
            // transform.position = Vector3.zero;
            transform.position = startPosition;
        }

        Debug.Log("New Position: " + transform.position);
    }
}
