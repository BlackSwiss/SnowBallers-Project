using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreKeeper scoreKeeper;

    void Start()
    {
        scoreKeeper.AddScore(new Score("test1", 10));
        scoreKeeper.AddScore(new Score("test2", 6));

        var scores = scoreKeeper.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.player.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}
