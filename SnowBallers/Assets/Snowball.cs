using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public AudioSource bonk;
    public AudioSource dizzy;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit!");
            collision.gameObject.GetComponent<Health>().decreaseHealth(1);
            //if(collision.gameObject.GetComponent<ChangeColor>())
            collision.gameObject.GetComponent<ChangeColor>().ChangeObjectColor();
            collision.gameObject.GetComponent<Health>().hitAnimation();

            foreach (Transform c in transform)
            {
                c.gameObject.GetComponent<ParticleSystem>().Play();
            }

            bonk.Play();
            dizzy.Play();

        }
    }
}
