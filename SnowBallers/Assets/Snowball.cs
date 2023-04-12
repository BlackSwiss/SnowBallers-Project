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

    public int scoreCount = 1;

    private void Start()
    {
        initialPos = gameObject.transform.position;        
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Headshot" && ownersID != collision.gameObject.GetComponent<PhotonView>().OwnerActorNr)
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
        else if(collision.gameObject.tag == "Player" && ownersID != collision.gameObject.GetComponent<PhotonView>().OwnerActorNr)
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = initialPos;
        }

        //If snowball gets owned by another player, we will assign the owner id to that players
        if(GetComponent<Photon.Pun.PhotonView>().OwnerActorNr != ownersID)
            ownersID =  GetComponent<Photon.Pun.PhotonView>().OwnerActorNr;
    }


}