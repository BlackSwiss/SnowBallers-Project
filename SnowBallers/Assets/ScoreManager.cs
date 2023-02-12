using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    public GameObject Canvas;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPlayerToScore(GameObject player)
    {
        players.Add(player);
        Instantiate(Resources.Load("Player Score") as GameObject, Canvas.transform);
    }
}
