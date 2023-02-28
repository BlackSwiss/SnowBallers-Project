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


    // Start is called before the first frame update
    void Start()
    {
        ScoreEvents.current.onPlayerHit += incrementScore;
    }

    [PunRPC]
    public void addPlayerToScore(GameObject player)
    {
        //Add player to our list of players (might get rid of this)
        //players.Add(player);

        //Create the player score on scoreboard and add it to list of scores
        GameObject playerScore = PhotonNetwork.Instantiate("Player Score",transform.position,transform.rotation);
        //Set the parent of the score to the scoreholder
        playerScore.GetPhotonView().RPC("setParentPhoton", RpcTarget.AllBuffered, "Score Holder");
        Debug.Log("RPC Called");

        //playerScores.Add(playerScore);
        //gameObject.GetPhotonView().RPC("addScoreToArray", RpcTarget.AllBuffered, playerScore.GetComponent<PlayerScore>());

        //Rebuild the layout group so our ui doesnt overlap
        ScoreHolder.GetPhotonView().RPC("rebuildLayout", RpcTarget.AllBuffered);

        //Add the player id to this specific score listing 
        //playerScore.GetComponent<PlayerScore>().syncPlayerID(player.GetComponent<NetworkPlayer>().playerID);
        playerScore.GetPhotonView().RPC("syncPlayerID",RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);



    }


    //Find and increment correct score
    public void incrementScore(int ownerID)
    {
       
        //foreach(GameObject playerScore in playerScores)
        //{
        //    PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();
        //    //If this player was the owner of the hit
        //    if (scoreScript.playerID == ownerID)
        //    {
        //        Debug.Log("Someone hit, incrementing score to " + ownerID);
        //        //Use the increment score script on object itself
        //        scoreScript.incrementScore();
        //    }
        //}
    }


}
