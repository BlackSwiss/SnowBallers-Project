using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerScore : MonoBehaviour
{
    public GameObject scoreText;
    public int score = 0;
    public int playerID;
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Player " + playerID + " Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void syncPlayerID(int currentPlayerID)
    {
        playerID = currentPlayerID;
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
