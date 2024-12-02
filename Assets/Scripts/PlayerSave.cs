using UnityEngine;
using System.Collections.Generic;

public class PlayerSave : MonoBehaviour
{
    private SaveSystem saveSystem;
    private ScoreManager scoreManager;

    // List to store the enemies dynamically
    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) // Save game on F5 press
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.F6)) // Load game on F6 press
        {
            LoadGame();
        }

        // Dynamically add newly spawned enemies (example)
        UpdateEnemyList();
    }

    // Function to update the enemies list dynamically
    private void UpdateEnemyList()
    {
        // Example: You would call this whenever enemies are spawned
        enemies.Clear(); // Clear existing list
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Assuming all enemies are tagged "Enemy"

        foreach (GameObject enemy in allEnemies)
        {
            if (!enemies.Contains(enemy)) // Avoid duplicates if enemies are already added
            {
                enemies.Add(enemy);
            }
        }
    }

    public void SaveGame()
    {
        // Get player position and score
        Vector3 playerPosition = transform.position;
        int playerScore = scoreManager.GetScore();

        // Save enemy positions
        saveSystem.SaveGameState(playerPosition, playerScore, enemies);
        Debug.Log($"Game Saved! Player Position: {playerPosition}, Score: {playerScore}");
    }

    public void LoadGame()
    {
        var data = saveSystem.LoadGameState();
        if (data != null)
        {
            // Load player position
            transform.position = data.position;
            scoreManager.SetScore(data.score);
            Debug.Log($"Loaded Position: {data.position}, Score: {data.score}");

            // Load enemy positions (assuming enemies have already been spawned)
            for (int i = 0; i < data.enemiesData.Count; i++)
            {
                if (i < enemies.Count && enemies[i] != null) // Ensure enemy exists
                {
                    enemies[i].transform.position = data.enemiesData[i].position;
                    Debug.Log($"Enemy {i} Loaded Position: {enemies[i].transform.position}");
                }
            }
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }
}
