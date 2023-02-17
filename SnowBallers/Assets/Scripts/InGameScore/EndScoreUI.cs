using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScoreUI : MonoBehaviour
{
    public EndRowUI rowUI;
    public ScoreManager scoreManager = new ScoreManager();

    

    void Start()
    {

        int playerCount = scoreManager.players.Count;
        for (int i = 0; i < playerCount; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<EndRowUI>();
            row.player.text = scoreManager.players[i].ToString();
            row.score.text = scoreManager.playerScores[i].ToString();
        }

    }
}
