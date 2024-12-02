using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    private SaveSystem saveSystem;
    private ScoreManager scoreManager;

    private void Start()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        scoreManager = FindObjectOfType<ScoreManager>();
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
        saveSystem.SavePlayerState(transform.position, scoreManager.GetScore());
        Debug.Log($"Game Saved! Position: {transform.position}, Score: {scoreManager.GetScore()}");
    }

    public void LoadGame()
    {
        var data = saveSystem.LoadPlayerState();
        if (data != null)
        {
            Debug.Log($"Loaded Position: {data.position}, Score: {data.score}");

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

            scoreManager.SetScore(data.score);

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

            Debug.Log($"Player Position After Teleport: {transform.position}, Score After Load: {scoreManager.GetScore()}");
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }
}
