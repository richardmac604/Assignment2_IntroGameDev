using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerTrigger : MonoBehaviour
{
    // This function is called when another collider enters this object's trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
       
        SceneManager.LoadSceneAsync("WinnerScene");
    }
}

