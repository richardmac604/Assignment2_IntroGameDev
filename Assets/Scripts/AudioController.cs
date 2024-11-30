using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController aCtrl;
    public AudioSource playerRunning;
    public AudioSource ballHitWall;
    public AudioSource playerHitWall;
    public AudioSource enemyRespawn;
    public AudioSource enemyDying;
    
    public AudioSource dayBGMusic;
    public AudioSource nightBGMusic;

    public AudioSource freeBird;

    private GameObject player;
    private GameObject enemy;
    private float maxDistance = 10f;
    private float minVolume = 0f;
    private float maxVolume = 1f;

    public void Awake()
    {
        if(aCtrl == null)
        {
            // levelMusic = bgMusic1.GetComponnt<AudioSource>();
            // levelMusic.loop = true;
            aCtrl = this;
        }
        player = GameObject.FindWithTag("Player");
    }

    public void PlayRunning(){
        aCtrl.playerRunning.Play();
    }

    public void StopRunning(){
        aCtrl.playerRunning.Stop();
    }

    public void PlayPlayerHitWall(){
        aCtrl.playerHitWall.Play();
    }
    public void PlayEnemyRespawning(){
        aCtrl.enemyRespawn.Play();
    }
    public void PlayEnemyDying(){
        aCtrl.enemyDying.Play();
    }

    // Proximity Audio for Enemy
    private void EnemyProximityAudio(){
        if (player == null || enemy == null){
            freeBird.volume = 0;
            enemy = GameObject.FindWithTag("Enemy");
        } else {
            float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

            float volume = Mathf.Clamp01(1 - (distance / maxDistance));
            freeBird.volume = Mathf.Lerp(minVolume, maxVolume, volume);
        }

        if (freeBird.volume == 0 && freeBird.isPlaying) {
            freeBird.Pause();
        } else if (freeBird.volume > 0 && !freeBird.isPlaying) {
            freeBird.UnPause();
        }
    }

    private void Update() {
        EnemyProximityAudio();
    }

    
}
