using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    Animator animator;

    public float speed = 2.0f;
    private float leftOrRight = 0f;
    public float checkDistance = 0.5f;
    public LayerMask wallLayer;
    private float turnTimer = 0f;
    private float turnCooldown = 1f;

    // Code for dying and respawn
    private GameObject enemyManager;
    private int health = 3;
    
    public void takeDamage(){
        --health;
    }

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
        animator = GetComponent<Animator>();
        enemyManager = GameObject.Find("EnemyManager");
    }
    
    void Update(){
        
        if (Input.GetKeyDown("space")){   
            Debug.Log("Space presed");
            enemyManager.GetComponent<EnemyManager>().EnemyDieSequence(gameObject);
        }
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(health <= 0) {
        //     dieAndRespawn();
        // }
        
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
            }
            
        }

        animator.SetFloat("InputX", leftOrRight);
        animator.SetFloat("InputY", speed);
    }
}
