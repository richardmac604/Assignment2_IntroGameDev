using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    [System.Serializable]
    public class EnemyData
    {
        public Vector3 position; // Save position of each enemy
    }

    [System.Serializable]
    public class PlayerData
    {
        public Vector3 position;
        public int score;
        public List<EnemyData> enemiesData; // List of enemy positions
    }

    public void SaveGameState(Vector3 playerPosition, int score, List<GameObject> enemies)
    {
        PlayerData data = new PlayerData
        {
            position = playerPosition,
            score = score,
            enemiesData = new List<EnemyData>()
        };

        // Save positions of enemies
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                data.enemiesData.Add(new EnemyData { position = enemy.transform.position });
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved: " + savePath);
    }

    public PlayerData LoadGameState()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            Debug.Log("Game Loaded");
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found!");
            return null;
        }
    }
}
