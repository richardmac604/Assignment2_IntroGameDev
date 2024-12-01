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

    public GameObject flashLight;
    private bool flashLightToggle;

    // For Proximity Audio
    private GameObject player;
    private GameObject enemy;
    private float maxDistance = 10f;
    private float minVolume = 0f;
    private float maxVolume = 1f;

    // For Day/Night Audio and play/pause
    private bool playBGM = true;
    private bool isItDay = true;

    // For Fog audio reduction
    private bool isItFoggy = false;
    private static float globalVolume = 1f;

    public void Awake()
    {
        if(aCtrl == null)
        {
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

    public void PlayBallHitWall(){
        aCtrl.ballHitWall.Play();
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
            freeBird.volume = Mathf.Lerp(minVolume, maxVolume, volume) * globalVolume;
        }

        if (freeBird.volume == 0 && freeBird.isPlaying) {
            freeBird.Pause();
        } else if (freeBird.volume > 0 && !freeBird.isPlaying) {
            freeBird.UnPause();
        }
    }

    private void ChangeBGMusic(){
        isItDay = !isItDay;
    }

    private void PlayBGMusic(){
        if(isItDay) {
            nightBGMusic.Pause();
            dayBGMusic.UnPause();
        } else {
            dayBGMusic.Pause();
            nightBGMusic.UnPause();
        }
    }

    private void PlayOrPauseBGM(){
        playBGM = !playBGM;
    }

    private void PauseAllBGM(){
        dayBGMusic.Pause();
        nightBGMusic.Pause();
    }

    // Change volume based on if it's foggy or not
    private void FoggyVolume(){
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        
        isItFoggy = !isItFoggy;
        
        // Set global volume multiplier
        // Only alternates between 0.5 and 1 for volume
        if(isItFoggy) {
            globalVolume = 0.5f;
            
            for(int i = 0; i < audioSources.Length; i++) {

                // Adjust volume accordingly
                audioSources[i].volume *= globalVolume;
            }

        } else {
            globalVolume = 1f;
            for(int i = 0; i < audioSources.Length; i++) {

                // Adjust volume accordingly
                audioSources[i].volume = maxVolume;
            }
        }
    }

    private void Update() {

        if(playBGM){
            PlayBGMusic();
        } else {
            PauseAllBGM();
        }

        EnemyProximityAudio();

        // Keycodes for fog, changing between day/night bgm, and play/pausing bgm
        if (Input.GetKeyDown(KeyCode.I)){   
            Debug.Log("I pressed");
            FoggyVolume();
        }
        if (Input.GetKeyDown(KeyCode.P)){   
            Debug.Log("P pressed");
            ChangeBGMusic();
        }
        if (Input.GetKeyDown(KeyCode.L)){   
            Debug.Log("L pressed");
            PlayOrPauseBGM();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            flashLightToggle = !flashLightToggle;
            flashLight.SetActive(flashLightToggle);
        }
    }

    
}
