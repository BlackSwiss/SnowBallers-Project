using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Unity.XR.CoreUtils;

using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{

    private GameObject spawnedPlayerPrefab;
    public GameObject scoreManager;
    public List<GameObject> players = new List<GameObject>();

    // Vars for spawn point index and managing XR camera position for spawned players 
    public int spawnPointIndex;
    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    // Vars for timer
    double startTime;
    double timerIncrement;
    double roundTime = 60;
    double timerDecrement;
    bool isTimerSet = false;
    bool isTimerOver = false;
    int timeBeforeKicked = 15;
    const int NumPlayersToStartMatch = 1;

    // Vars for custom lobby
    public BoolSO isCustomLobby;
    public CustomLobbyManager customLobbyManager;
    int changeTimeStep = 5;
    int minimumTime = 30;
    int maximumTime = 180;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isTimerSet && !isTimerOver)
        {
            //Set increment as difference from current time and start time
            timerIncrement = PhotonNetwork.Time - startTime; 
            timerDecrement = roundTime - timerIncrement;
            Debug.Log("timerDecrement " + timerDecrement);
            if(timerDecrement <= 0 && !isTimerOver)
            {
                // Call function for when time is up
                OnTimeIsUp();
                Debug.Log("Time is up!");
                isTimerOver = true;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        //Set index for spawn point array
        spawnPointIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        base.OnJoinedRoom();

        //Spawn player model and controls
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", SpawnManager.instance.spawnPoints[spawnPointIndex].position, SpawnManager.instance.spawnPoints[spawnPointIndex].rotation);
        
        //Set XR origin Transform position to spawn point's position so that spawned player's camera is matching spawn point direction .
        XROrigin rig = FindObjectOfType<XROrigin>();
        rig.transform.position = SpawnManager.instance.spawnPoints[spawnPointIndex].position;
        rig.transform.rotation = SpawnManager.instance.spawnPoints[spawnPointIndex].rotation;
        headRig = rig.transform.Find("Camera Offset/Main Camera");
        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

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

        // Have second player start timer setting for all players
        if (PhotonNetwork.CurrentRoom.PlayerCount >= NumPlayersToStartMatch)
        {
            if(isCustomLobby.Value)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    Invoke(nameof(customLobbyInvoke),9);
                }
            }
            else
            {
                SetTimerRPC();
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause) 
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
        Debug.Log("Left room.");
    }

    public void LeaveRoom() 
    {
        if(PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
    }

    public List<GameObject> getPlayers()
    {
        return players;
    }

    // Called when round timer reaches 0
    public void OnTimeIsUp()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        GameObject networkPlayer = GameObject.Find("Network Manager");
        scoreManager.GetComponent<PhotonView>().RPC("endGame", RpcTarget.AllBuffered);
        networkPlayer.GetComponent<PhotonView>().RPC("KickPlayersFromRoom", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void KickPlayersFromRoom()
    {
        Invoke(nameof(LeaveRoom), timeBeforeKicked);
    }

    [PunRPC]
    public void SetTimer()
    {
        startTime = PhotonNetwork.Time;
        isTimerSet = true;
    }

    public double getTimerDecrement()
    {
        return timerDecrement;
    }

    public void SetTimerRPC()
    {
        GameObject networkPlayer = GameObject.Find("Network Manager");
        networkPlayer.GetComponent<PhotonView>().RPC("SetTimer", RpcTarget.AllBuffered);
    }

    public void incrementRoundTime()
    {
        roundTime += changeTimeStep;
        if(roundTime > maximumTime)
            roundTime = maximumTime;
    }

    public void decrementRoundTime()
    {
        roundTime -= changeTimeStep;
        if(roundTime < minimumTime)
            roundTime = minimumTime;
    }

    public double getStartTime()
    {
        return roundTime;
    }

    public void customLobbyInvoke()
    {
        customLobbyManager.enableCustomLobbyUI();
    }
}
