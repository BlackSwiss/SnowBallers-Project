using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class PodiumManager : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;
    public Transform[] spawnPointsToAdd;
    public AudioSource fightMusic;
    public AudioSource podiumSound;

    //Disables unnecessary game objects for end game state
    private void disableObjects()
    {
        foreach(GameObject currentObject in objectsToDisable)
        {
            currentObject.SetActive(false);
        }
    }

    //Enables necessary game objects for end game state
    private void enableObjects()
    {
        foreach(GameObject currentObject in objectsToEnable)
        {
            currentObject.SetActive(true);
        }
    }

    //Replaces spawn points with podium spawn points
    private void addSpawnPointsToManager()
    {
        SpawnManager.instance.spawnPoints = spawnPointsToAdd;
    }

    //Moves player to appropriate spawn point on podium
    private void movePlayerToPodium()
    {
        XROrigin rig = FindObjectOfType<XROrigin>();
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        NetworkPlayer networkPlayer = FindObjectOfType<NetworkPlayer>();
        CinemaManager cinemaManager = FindObjectOfType<CinemaManager>();

        //Stops function if no network player is found
        if(networkPlayer == null)
            return;

        int playerID = networkPlayer.playerID;
        //If current player is 1st place
        if(playerID == scoreManager.topScoresID[0])
        {
            //Move current player to 1st place spawn point
            rig.transform.position = SpawnManager.instance.spawnPoints[0].position;
            rig.transform.rotation = SpawnManager.instance.spawnPoints[0].rotation;
        }
        //If current player is 2nd place
        else if(playerID == scoreManager.topScoresID[1])
        {
            //Move current player to 2nd place spawn point
            rig.transform.position = SpawnManager.instance.spawnPoints[1].position;
            rig.transform.rotation = SpawnManager.instance.spawnPoints[1].rotation;
        }
        //If current player is 3rd place
        else if(playerID == scoreManager.topScoresID[2])
        {
            //Move current player to 3rd place spawn point
            rig.transform.position = SpawnManager.instance.spawnPoints[2].position;
            rig.transform.rotation = SpawnManager.instance.spawnPoints[2].rotation;
        }
        //If current player is not in top 3 players
        else{
            //Move current player to podium audience spawn point
            rig.transform.position = SpawnManager.instance.spawnPoints[3].position;
            rig.transform.rotation = SpawnManager.instance.spawnPoints[3].rotation;
        }

        //Rotate player to face podium mirror
        rig.transform.LookAt(objectsToEnable[1].transform);
        //Sync POV camera for outro cutscene
        cinemaManager.syncPOVCamera(rig.transform.Find("Camera Offset/Main Camera"));
    }

    private void disableMovement()
    {
        GameObject locomotion = GameObject.Find("Locomotion System");
        ActionBasedContinuousMoveProvider moveProvider = locomotion.GetComponentInChildren<ActionBasedContinuousMoveProvider>();
        moveProvider.enabled = false;
    }

    //Transitions to end game podium state
    public void swapToPodium()
    {
        disableObjects();
        enableObjects();
        addSpawnPointsToManager();
        movePlayerToPodium();
        disableMovement();
        fightMusic.Stop();
        podiumSound.Play();
    }
}
