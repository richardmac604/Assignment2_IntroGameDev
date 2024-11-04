using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    private GameObject closestPlayerObj = null;
    Animator animator;

    public float speed = 2.0f;
    private float leftOrRight = 0f;
    public float checkDistance = 0.5f;
    public LayerMask wallLayer;
    private float turnTimer = 0f;
    private float turnCooldown = 1f;
    

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

        //Check which paths don't have a wall
        for(int i = 0; i < checkPaths.Length; i++) {
            if(!isWallInFront(checkPaths[i])){
                availablePaths.Add(checkPaths[i]);
            }
        }


        //Pick random direction out of available paths and return it
        int randomDirection = (int) Random.Range(0, availablePaths.Count);

        return availablePaths[randomDirection];

    }

    // private Vector3 randomizeDirection(){

    // }


    // Start is called before the first frame update
    void Start()
    {
        closestPlayerObj = GameObject.FindGameObjectWithTag("Player1");
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        turnTimer += Time.deltaTime;
        //Check if wall is in front
        if(isWallInFront(transform.forward)) {
            Debug.Log("Wall detected");

            // transform.LookAt(newDirection());
            transform.forward = newDirection();

            //Implement change to random direction here
        }

        //Randomize path if new path becomes available

        else if( turnTimer >= turnCooldown){
            if(!isWallInFront(transform.right) || !isWallInFront(-transform.right)) {
                Debug.Log("Turn randomly?");

                Vector3 turnDirection = newDirection();

                if(turnDirection != transform.forward && turnDirection != -transform.forward) {
                    transform.forward = turnDirection;
                }
                turnTimer = 0f;

                // transform.position = new Vector3(((float) (int) (transform.position.x + 0.5f)), 0f, ((float) (int) (transform.position.z + 0.5f)));
                // transform.position.z = (float) (int) transform.position.z;
            }
            
        }
        

        // else if((transform.position.x % 1 < 0.01) || (transform.position.z % 1 < 0.01)){
        //     if(!isWallInFront(transform.right) || !isWallInFront(-transform.right)) {
        //         Debug.Log("Turn randomly?");

        //         Vector3 turnDirection = newDirection();

        //         if(turnDirection != transform.forward && turnDirection != -transform.forward) {
        //             transform.forward = turnDirection;
        //         }

        //         transform.position = new Vector3(((float) (int) transform.position.x + 0.5f), 0f, ((float) (int) transform.position.z));
        //         // transform.position.z = (float) (int) transform.position.z;
        //     }
        // }
        
        // transform.LookAt(closestPlayerObj.transform);

        animator.SetFloat("InputX", leftOrRight);
        animator.SetFloat("InputY", speed);
    }
}
