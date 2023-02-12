using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;
    public int playerID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void syncPlayerID(int currentPlayerID)
    {
        playerID = currentPlayerID;
    }

    public void incrementScore()
    {
        score += 1;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
