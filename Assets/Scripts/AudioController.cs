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

    public void PlayDayBGM(){
        aCtrl.dayBGMusic.Play();
    }
    public void PauseDayBGM(){
        aCtrl.dayBGMusic.Pause();
    }

    public void PlayNightBGM(){
        aCtrl.nightBGMusic.Play();
    }
    public void PauseNightBGM(){
        aCtrl.nightBGMusic.Pause();
    }

    // Proximity Audio for Enemy
    private void EnemyProximityAudio(){
        // Set audio to 0 if either 
        // 1) Player doesn't exist
        // 2) Enemy doesn't exist (not spawned yet initially)
        // 3) Enemy is inactive
        if (player == null || enemy == null || !enemy.activeSelf){
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
