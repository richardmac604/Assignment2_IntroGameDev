using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    private float speed = 10.0f;
    private TwoPlayerInput inputActions;
    private InputAction movement;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //get rigidbody, responsible for enabling collision with other colliders
        inputActions = new TwoPlayerInput();
    }

    private void OnEnable(){
        movement = inputActions.Player2.Movement;
        movement.Enable();
    }

    private void OnDisable(){
        movement.Disable();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 v2 = movement.ReadValue<Vector2>();
        Vector3 v3 = new Vector3(v2.x * speed, 0, v2.y * speed);

        // transform.Translate(v3);
        rb.velocity = v3;
        // rb.MovePosition(transform.position + v3 * speed * Time.deltaTime);
        // transform.Translate(v3 * speed * Time.deltaTime);

    }
}
