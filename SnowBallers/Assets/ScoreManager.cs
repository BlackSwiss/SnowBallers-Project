using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> playerScores = new List<GameObject>();
    public GameObject Canvas;


    // Start is called before the first frame update
    void Start()
    {
        ScoreEvents.current.onPlayerHit += incrementScore;
    }



    public void addPlayerToScore(GameObject player)
    {
        //Add player to our list of players (might get rid of this)
        players.Add(player);

        //Create the player score on scoreboard and add it to list of scores
        GameObject playerScore = Instantiate(Resources.Load("Player Score") as GameObject, Canvas.transform);
        playerScores.Add(playerScore);

        //Add the player id to this specific score listing 
        playerScore.GetComponent<PlayerScore>().syncPlayerID(player.GetComponent<NetworkPlayer>().playerID);
        
        
    }

    //Find and increment correct score
    public void incrementScore(int ownerID)
    {
       
        foreach(GameObject playerScore in playerScores)
        {
            PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();
            //If this player was the owner of the hit
            if (scoreScript.playerID == ownerID)
            {
                Debug.Log("Someone hit, incrementing score to " + ownerID);
                //Use the increment score script on object itself
                scoreScript.incrementScore();
            }
        }
    }


}
