using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawnMenu : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent bot;

    //void Start()
    //{
    //    bot = NavMeshAgent.GetComponent<UnityEngine.AI.NavMeshAgent>();
    //}

    public void OnClickSpawnBot()
    {
        int spawnBotPoint = 1;
        Vector3 pos = SpawnManager.instance.spawnPoints[spawnBotPoint].position;
        bot.Warp(pos);
        Debug.Log("spawn bot pressed");
    }

    public void OnClickDeleteBot()
    {
        Vector3 posDelete = GameObject.Find("BotStorage").transform.position;
        bot.Warp(posDelete);
        Debug.Log("delete bot pressed");
    }
}
