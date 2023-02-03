using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public AudioSource bonk;
    public AudioSource dizzy;

    public Vector3 initialPos;
    private void Start()
    {
        initialPos = gameObject.transform.position;        
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit!");
            if (collision.gameObject.GetComponent<Health>())
                collision.gameObject.GetComponent<Health>().decreaseHealth(1);
            //if(collision.gameObject.GetComponent<ChangeColor>())
            if(collision.gameObject.GetComponent<ChangeColor>())
                collision.gameObject.GetComponent<ChangeColor>().ChangeObjectColor();
            if (collision.gameObject.GetComponent<Health>())
                collision.gameObject.GetComponent<Health>().hitAnimation();

            foreach (Transform c in transform)
            {
                c.gameObject.GetComponent<ParticleSystem>().Play();
            }

            bonk.Play();
            dizzy.Play();

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.position = initialPos;
        }
    }
}