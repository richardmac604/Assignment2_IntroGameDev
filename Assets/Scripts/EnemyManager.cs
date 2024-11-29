using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
        // Code for dying and respawn
    private int health = 3;
    private Coroutine stopwatchCoroutine;
    private float respawnTimer;
    private int respawnCooldown = 3;

     private IEnumerator dieAndRespawn(GameObject enemy){
        //Play audio clip
        //Disable enemy object
        enemy.SetActive(false);
        //Coroutine timer, respawn after 5seconds
        yield return new WaitForSeconds(respawnCooldown);
        Respawn(enemy);
    }

    private void Respawn(GameObject enemy){
        //Enable object to simulate respawn
        //Randomize "spawn" position
        enemy.SetActive(true);
        health = 3;
    }

    public void EnemyDieSequence(GameObject enemy){
        StartCoroutine(dieAndRespawn(enemy));
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
