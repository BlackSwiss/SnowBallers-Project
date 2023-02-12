using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{

    private GameObject spawnedPlayerPrefab;
    public GameObject scoreManager;

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
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
