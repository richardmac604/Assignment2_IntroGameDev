using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;

    public void IncreaseScore()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            score++;
        }
        Debug.Log("Score Increased: " + score);
        UpdateScoreDisplay();
    }


    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score Reset: " + score);
        UpdateScoreDisplay();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        Debug.Log("Score Set: " + score);
        UpdateScoreDisplay();
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score;
    }

    void Start()
    {
        UpdateScoreDisplay();
    }

    void Update()
    {
        IncreaseScore();
    }
}
