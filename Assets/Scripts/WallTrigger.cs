using UnityEngine;
using UnityEngine.SceneManagement;

public class WallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player interacts with the trigger
        {
            SceneManager.LoadScene("WinnerScene"); // Replace with your next scene name
        }
    }
}