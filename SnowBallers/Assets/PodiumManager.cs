using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumManager : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;
    public Transform[] spawnPointsToAdd;
    public SpawnManager spawnManager;
    public ScoreManager scoreManager;
    public AudioSource drumRoll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void disableObjects()
    {
        foreach(GameObject currentObject in objectsToDisable)
        {
            currentObject.SetActive(false);
        }
    }

    private void enableObjects()
    {
        foreach(GameObject currentObject in objectsToEnable)
        {
            currentObject.SetActive(true);
        }
    }

    private void addSpawnPointsToManager()
    {
        spawnManager.spawnPoints = spawnPointsToAdd;
    }

    public void swapToPodium()
    {
        disableObjects();
        enableObjects();
        addSpawnPointsToManager();
        drumRoll.Play();
    }
}
