using UnityEngine;
using UnityEngine.InputSystem;

public class BallThrower : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform throwPoint;
    public Camera playerCamera;

    public float throwForce = 15f;

    private Movement playerControls;

    private void Awake()
    {
        playerControls = new Movement();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.ThrowBall.performed += OnThrowPerformed;
    }

    private void OnDisable()
    {
        playerControls.Player.ThrowBall.performed -= OnThrowPerformed;
        playerControls.Disable();
    }

    private void OnThrowPerformed(InputAction.CallbackContext context)
    {
        ThrowBall();
    }

    private void ThrowBall()
    {
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, throwPoint.rotation);

        Vector3 throwDirection = playerCamera.transform.forward;

        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
            ballRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }
}
