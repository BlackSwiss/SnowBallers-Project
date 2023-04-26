using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviourPun
{
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> playerScores = new List<GameObject>();
    public GameObject ScoreHolder;
    public GameObject ScoreHolderHUD;
    public AudioSource gameOverSound;

    //Variables for end game podium
    public int[] topScores = {-1,-1,-1};
    public int[] topScoresID = {-1,-1,-1};
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        ScoreEvents.current.onPlayerHit += incrementScore;
        ScoreEvents.current.onHeadshot += incrementScoreHeadshot;
        //Invoke(nameof(endGame), 5);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate top players for end game podium if there is at least one player
        if(playerScores.Count > 0 && !gameOver)
        {
            calculateTopPlayers();
        }
    }

    //Keeps track of top 3 players for end game podium
    private void calculateTopPlayers()
    {
        foreach (GameObject playerScore in playerScores)
        {
            //Get current player score
            PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();

            //If current score is greater than 1st place score
            if(scoreScript.score >= topScores[0])
            {
                //Replace 1st place from topScores
                topScores[0] = scoreScript.score;
                topScoresID[0] = scoreScript.playerID;
            }
            //If current score is greater than 2nd place score
            else if(scoreScript.score >= topScores[1])
            {
                //Replace 2nd place from topScores
                topScores[1] = scoreScript.score;
                topScoresID[1] = scoreScript.playerID;
            }
            //If current score is greater than 3rd place score
            else if(scoreScript.score >= topScores[2])
            {
                //Replace 3rd place from topScores
                topScores[2] = scoreScript.score;
                topScoresID[2] = scoreScript.playerID;
            }
        }
    }

    [PunRPC]
    public void addPlayerToScore(GameObject player)
    {
        //Add player to our list of players (might get rid of this)
        //players.Add(player);

        //Create the player score on scoreboard and add it to list of scores
        GameObject playerScore = PhotonNetwork.Instantiate("Player Score",transform.position,transform.rotation);
        GameObject playerScoreHUD = PhotonNetwork.Instantiate("Player Score",transform.position,transform.rotation);
        //Set the parent of the score to the scoreholder
        playerScore.GetPhotonView().RPC("setParentPhoton", RpcTarget.AllBuffered, "Score Holder");
        playerScoreHUD.GetPhotonView().RPC("setParentPhoton", RpcTarget.AllBuffered, "Score Holder HUD");
        Debug.Log("RPC Called");

        //playerScores.Add(playerScore);
        //gameObject.GetPhotonView().RPC("addScoreToArray", RpcTarget.AllBuffered, playerScore.GetComponent<PlayerScore>());

        //Rebuild the layout group so our ui doesnt overlap
        ScoreHolder.GetPhotonView().RPC("rebuildLayout", RpcTarget.AllBuffered);
        ScoreHolderHUD.GetPhotonView().RPC("rebuildLayout", RpcTarget.AllBuffered);

        //Add the player id to this specific score listing 
        //playerScore.GetComponent<PlayerScore>().syncPlayerID(player.GetComponent<NetworkPlayer>().playerID);
        playerScore.GetPhotonView().RPC("syncPlayerID",RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        playerScoreHUD.GetPhotonView().RPC("syncPlayerID",RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
    }


    //Find and increment correct score
    public void incrementScore(int ownerID, int scoreCount)
    {
        foreach (GameObject playerScore in playerScores)
        {
            PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();
            //If this player was the owner of the hit
            if (scoreScript.playerID == ownerID)
            {
                Debug.Log("Someone hit, incrementing score to " + ownerID);
                //Use the increment score script on object itself
                for(int i = 0; i < scoreCount; i++)
                    scoreScript.incrementScore();
            }
        }
    }

    //Find and increment correct score for headshot
    public void incrementScoreHeadshot(int ownerID)
    {
        foreach (GameObject playerScore in playerScores)
        {
            PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();
            //If this player was the owner of the hit
            if (scoreScript.playerID == ownerID)
            {
                Debug.Log("Someone hit, incrementing score to " + ownerID);
                //Use the increment score script on object itself
                scoreScript.incrementScore();
                scoreScript.incrementScore();
            }
        }
    }

    //Wrapper function used with invoke in endGame() to delay end game transition
    private void endGameInvoke()
    {
        PodiumManager podiumManager = FindObjectOfType<PodiumManager>();
        podiumManager.swapToPodium();
    }

    //Transitions to end game state
    public void endGame()
    {
        Debug.Log("Game over, transitioning to end game podium");
        gameOver = true;
        gameOverSound.Play();
        //Delays transition by 5 seconds to allow gameOverSound to finish playing
        Invoke(nameof(endGameInvoke), 5);
    }
}
