using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    private ScoreData sd;

    void Awake()
    {
        sd = new ScoreData();
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
    }

    // Update is called once per frame
    void UpdateScore(int addPoint) {
        score += addPoint;
        scoreText.text = "Score: " + score;
    }
}
