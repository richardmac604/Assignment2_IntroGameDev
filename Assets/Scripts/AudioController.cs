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

    public AudioSource sfxSrc;
    private AudioSource levelMusic;

    // Start is called before the first frame update
    public void Awake()
    {
        if(aCtrl == null)
        {
            // levelMusic = bgMusic1.GetComponnt<AudioSource>();
            // levelMusic.loop = true;
            aCtrl = this;
        }
    }

    public void PlayRunning(){
        aCtrl.playerRunning.Play();
    }

    public void StopRunning(){
        aCtrl.playerRunning.Stop();
    }
}
