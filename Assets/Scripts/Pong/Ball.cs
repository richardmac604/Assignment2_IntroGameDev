using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private float speed = 15.0f;
    private float maxAngle = 60f;
    private float paddleInfluenceFactor = 0.2f;
    private Vector3 velocity;
    private int maxScore = 5;
    private int player1Score = 0;
    private int player2Score = 0;
    bool exitToMenu = false;
    private float timeElapsed = 0;
    private float delayBeforeLoading = 3.0f;

    Vector3 spawnPoint;
    Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        velocity = new Vector3(0, 0, -1);
        rb = GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision){
        Rigidbody otherRigidbody = collision.rigidbody;

        if(otherRigidbody != null) {
            
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 paddleCenter = collision.collider.bounds.center;
            
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflectedVelocity = Vector3.Reflect(velocity, normal);

            float paddleWidth = collision.collider.bounds.size.x; 
            float offsetFromCenter = (contactPoint.x - paddleCenter.x) / (paddleWidth / 2);

            float reflectionAngle = offsetFromCenter * maxAngle;
            reflectedVelocity.x = Mathf.Sin(reflectionAngle * Mathf.Deg2Rad) * reflectedVelocity.magnitude;

            reflectedVelocity.x += otherRigidbody.velocity.x * paddleInfluenceFactor;

            velocity = reflectedVelocity.normalized;

        }    
        else{
            velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
        }
    }

    private void OnTriggerEnter(Collider other){

        if(other.gameObject.tag == "Goal"){
            Debug.Log("Goal hit");

            GetComponent<Renderer>().enabled = false;
            
            if(other.gameObject.transform.position.z < 0) {
                //player 1 scored
                player1Score++;
                updateScore("Player2Score", player1Score);

                
            } else {
                //player 2 scored
                player2Score++;
                updateScore("Player1Score", player2Score);
            }

            if(player1Score >= maxScore || player2Score >= maxScore){
                //stop game here
                string winner;

                if(player1Score > player2Score) {
                    winner = "Red";
                } else{
                    winner = "Blue";
                }

                GameObject text = GameObject.Find("Winner");
                text.GetComponent<TMP_Text>().text = winner + " Wins!";

                //Delay then return to menu
                exitToMenu = true;


            }

            resetBall(other.gameObject.transform.position.z);

            //Figure out who scored
        }
    }

    private void resetBall(float whoScored){
        transform.position = spawnPoint;
        velocity = new Vector3(0, 0, whoScored).normalized;
        GetComponent<Renderer>().enabled = true;
    }

    private void updateScore(string playerScore, int updatedScore){
        GameObject text = GameObject.Find(playerScore);

        text.GetComponent<TMP_Text>().text = updatedScore.ToString();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += velocity * speed * Time.deltaTime;

        if(exitToMenu){
            timeElapsed += Time.deltaTime;
            if(timeElapsed > delayBeforeLoading){
                SceneManager.LoadScene("SampleScene");
            }
        }

        // Debug.Log(transform.position);
    }

    public void setSpeed(float newSpeed){
        speed = newSpeed;
    }
}
