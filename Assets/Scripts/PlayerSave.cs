using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerSave : MonoBehaviour
{
    private SaveSystem saveSystem;

    private void Start()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        saveSystem.SavePlayerState(transform.position);
        Debug.Log($"Game Saved! Position: {transform.position}");
    }


    public void LoadGame()
    {
        var data = saveSystem.LoadPlayerState();
        if (data != null)
        {
            Debug.Log($"Loaded Position: {data.position}");

            // Disable movement or controller components temporarily
            PlayerMovement controller = GetComponent<PlayerMovement>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            CharacterController charController = GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            transform.position = data.position;

            if (rb != null)
            {
                rb.isKinematic = false;
            }

            if (charController != null)
            {
                charController.enabled = true;
            }

            if (controller != null)
            {
                controller.enabled = true;
            }

            Debug.Log($"Player Position After Teleport: {transform.position}");
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }



}
