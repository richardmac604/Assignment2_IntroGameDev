using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    private GameObject closestPlayerObj = null;
    Animator animator;

    public float speed = 2.0f;
    private float minDist = 1f;
    private float leftOrRight = 0f;
    public float checkDistance = 0.5f;
    public LayerMask wallLayer;
    

    // Returns the distance between the object the script is attached to, and the targetObject
    // Takes in a single GameObject and returns a float representing the distance
    private float findDistance(GameObject targetObject){
        return Vector3.Distance(transform.position, targetObject.transform.position);
    }

    private bool isWallInFront(Vector3 viewDirection){
        // Vector3 forwardView = transform.forward;
        Ray ray = new Ray(transform.position, viewDirection);

        return Physics.Raycast(ray, checkDistance, wallLayer);
    }

    private Vector3 newDirection(){
        List<Vector3> availablePaths = new List<Vector3>();
        
        Vector3 up      = new Vector3(0, 0, 1);
        Vector3 down    = new Vector3(0, 0, -1);
        Vector3 left    = new Vector3(-1, 0, 0);
        Vector3 right   = new Vector3(1, 0, 0);

        Vector3[] checkPaths = new Vector3[] {up, down, left, right};

        // Vector3 checkPaths = new Vector3[] {Vector3.forward, down, left, right};

        //Check which paths don't have a wall
        for(int i = 0; i < checkPaths.Length; i++) {
            if(!isWallInFront(checkPaths[i])){
                availablePaths.Add(checkPaths[i]);
            }
        }

        int randomDirection = (int) Random.Range(0, availablePaths.Count);

        Debug.Log(randomDirection);
        Debug.Log(availablePaths[randomDirection]);
        return availablePaths[randomDirection];

    }


    // Start is called before the first frame update
    void Start()
    {
        closestPlayerObj = GameObject.FindGameObjectWithTag("Player1");
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Check if wall is in front
        if(isWallInFront(transform.forward)) {
            Debug.Log("Wall detected");

            // transform.LookAt(newDirection());
            transform.forward = newDirection();

            //Implement change to random direction here
        }
        
        // transform.LookAt(closestPlayerObj.transform);

        animator.SetFloat("InputX", leftOrRight);
        animator.SetFloat("InputY", speed);
    }
}
