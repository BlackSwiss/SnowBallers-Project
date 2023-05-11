using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerScore : MonoBehaviourPun
{
    public GameObject scoreText;
    public int score = 0;
    public int playerID;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        //scoreText.GetComponent<TextMeshProUGUI>().text = "Player " + playerID + " Score: " + score.ToString();
        ScoreManager scoreManager = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        foreach(GameObject playerScore in scoreManager.playerScores)
        {
            PlayerScore scoreScript = playerScore.GetComponent<PlayerScore>();
            if(this.playerID == scoreScript.playerID)
                return;
        }
        scoreManager.playerScores.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void syncPlayerID(int currentPlayerID)
    {
        playerID = currentPlayerID;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Player " + playerID + " Score: " + score.ToString();
    }

    [PunRPC]
    public void incrementScore()
    {
        score += 1;
        scoreText.GetComponent<TextMeshProUGUI>().text = "Player " + playerID + " Score: " + score.ToString();
    }

    [PunRPC]
    public void setParentPhoton(string parent)
    {
        Debug.Log("Parented");
        GameObject parentObj = GameObject.Find(parent);
        Debug.Log("Parent object: " + parentObj.name);
        gameObject.transform.SetParent(parentObj.transform,false);
    }


}
