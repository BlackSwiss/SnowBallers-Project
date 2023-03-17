using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{

    private GameObject spawnedPlayerPrefab;
    public GameObject scoreManager;
    public List<GameObject> players = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //Spawn player model and controls
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);

        //Give them an id so we can keep track of score
        spawnedPlayerPrefab.GetComponent<NetworkPlayer>().playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        //Add their score to the scoreboard in game
        scoreManager.GetComponent<ScoreManager>().addPlayerToScore(spawnedPlayerPrefab);

        Debug.Log("Added score to scoreboard");

        //Set players to player layer
        spawnedPlayerPrefab.layer = 7;

        //Add players to List of players
        players.Add(spawnedPlayerPrefab);
        Debug.Log("Player added to list");


    }

    public override void OnDisconnected(DisconnectCause cause) 
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
        Debug.Log("Left room.");
    }

    public void LeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
    }

    public List<GameObject> getPlayers()
    {
        return players;
    }
}
