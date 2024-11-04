using UnityEngine;
public class Return : MonoBehaviour
{
    private Movement controls;
    private CharacterController characterController;
    private Rigidbody rb;

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

    private void TeleportToOrigin()
    {
        Debug.Log("Teleporting");
        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = Vector3.zero;
            characterController.enabled = true;
        }
        else if (rb != null)
        {
            rb.MovePosition(Vector3.zero);
        }
        else
        {
            transform.position = Vector3.zero;
        }

        Debug.Log("New Position: " + transform.position);
    }
}
