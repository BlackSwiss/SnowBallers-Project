using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Snowball : MonoBehaviour
{
    public AudioSource snowballHit;
    public AudioSource bonk;
    public AudioSource dizzy;

    public Vector3 initialPos;

    public int ownersID = 0;
    public Collider owner;

    public int scoreCount = 1;

    private void Start()
    {
        
        initialPos = gameObject.transform.position;
        StartCoroutine(waitForCollider());
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<Health>() != null && collision.gameObject.GetComponent<Health>().networkPlayer != null)
        {
            Debug.Log("Player ID: " + collision.gameObject.GetComponent<Health>().networkPlayer.playerID
                + "Actor number: " + PhotonNetwork.LocalPlayer.ActorNumber);

            if (GetComponent<PhotonView>().IsMine || GetComponent<PhotonView>().AmOwner || collision.gameObject.GetComponent<Health>().networkPlayer.playerID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log("Hit owner");
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);
            }
            if (collision.gameObject.tag == "Headshot" && collision.gameObject.GetComponent<Health>().networkPlayer.playerID != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log("Player headshot!");
                //This will prevent a snowball that no one threw to do damage
                if (ownersID != 0)
                {
                    ScoreEvents.current.headshot(ownersID);
                    Debug.Log("Headshot");
                }

                if (collision.gameObject.GetComponent<Health>())
                {
                    //collision.gameObject.GetComponent<Health>().decreaseHealth(1);
                    collision.gameObject.GetComponent<Health>().hitAnimation();
                }
                //if(collision.gameObject.GetComponent<ChangeColor>())
                if (collision.gameObject.GetComponent<ChangeColor>())
                    collision.gameObject.GetComponent<ChangeColor>().ChangeObjectColor();


                foreach (Transform c in transform)
                {
                    c.gameObject.GetComponent<ParticleSystem>().Play();
                }

                bonk.Play();
                dizzy.Play();
            }
            else if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Health>().networkPlayer.playerID != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log("Player hit!");
                //This will prevent a snowball that no one threw to do damage
                if (ownersID != 0)
                {
                    ScoreEvents.current.playerHit(ownersID, scoreCount);
                    Debug.Log("Hit");
                }

                if (collision.gameObject.GetComponent<Health>())
                {
                    //collision.gameObject.GetComponent<Health>().decreaseHealth(1);
                    collision.gameObject.GetComponent<Health>().hitAnimation();
                }
                //if(collision.gameObject.GetComponent<ChangeColor>())
                if (collision.gameObject.GetComponent<ChangeColor>())
                    collision.gameObject.GetComponent<ChangeColor>().ChangeObjectColor();


                foreach (Transform c in transform)
                {
                    c.gameObject.GetComponent<ParticleSystem>().Play();
                }

                snowballHit.Play();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = initialPos;
        }

        //If snowball gets owned by another player, we will assign the owner id to that players
        if (GetComponent<Photon.Pun.PhotonView>().OwnerActorNr != ownersID)
        {
            ownersID = GetComponent<Photon.Pun.PhotonView>().OwnerActorNr;
            
        }
    }

    IEnumerator waitForCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
    }
   


}