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
    public AudioController aCtrl;

     private IEnumerator dieAndRespawn(GameObject enemy){
        //Play audio clip
        aCtrl.PlayEnemyDying();
        //Disable enemy object
        enemy.SetActive(false);
        //Coroutine timer, respawn after 5seconds
        yield return new WaitForSeconds(respawnCooldown);
        Respawn(enemy);
        aCtrl.PlayEnemyRespawning();
    }

    private void Respawn(GameObject enemy){
        //Enable object to simulate respawn
        //Randomize "spawn" position
        enemy.GetComponent<BasicAI>().resetHealth();
        enemy.transform.position = RandomSpawnPosition();
        enemy.SetActive(true);
    }

    // Used by enemy script
    public void EnemyDieSequence(GameObject enemy){
        StartCoroutine(dieAndRespawn(enemy));
    }

    private Vector3 RandomSpawnPosition(){
        return new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
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
