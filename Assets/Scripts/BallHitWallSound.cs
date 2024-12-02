using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitWallSound : MonoBehaviour
{
    public GameObject audioControllerObject;

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 3) {
            audioControllerObject.GetComponent<AudioController>().PlayBallHitWall();
        }
    }
}
