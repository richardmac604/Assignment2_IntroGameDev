using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPongAI : MonoBehaviour
{
    private GameObject ball = null;

    private GameObject closestPlayerObj = null;
    private float speed = 6.0f;
    private float minDist = 1f;

    // Returns the distance between the object the script is attached to, and the targetObject
    // Takes in a single GameObject and returns a float representing the distance
    private float findDistance(GameObject targetObject){
        return Vector3.Distance(transform.position, targetObject.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindWithTag("Ball");
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(playerObj.transform.position);

        if(ball.transform.position.x < transform.position.x) {
            transform.position -= transform.right * speed * Time.deltaTime;
        } else {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        // Debug.Log(transform.position);
    }
}
