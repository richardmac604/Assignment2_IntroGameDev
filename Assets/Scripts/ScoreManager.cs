using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;
    public void increaseScore(){
        score++;
        UpdateScoreDisplay();
    }

    private void resetScore(){
        score = 0;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay(){
        scoreText.text = "Score: " + score;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
