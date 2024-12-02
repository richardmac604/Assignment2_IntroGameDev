using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    [System.Serializable]
    public class PlayerData
    {
        public Vector3 position;
        public int score;
    }

    // Save the player state and score
    public void SavePlayerState(Vector3 position, int score)
    {
        PlayerData data = new PlayerData
        {
            position = position,
            score = score
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved: " + savePath);
    }

    // Load player state and score
    public PlayerData LoadPlayerState()
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
