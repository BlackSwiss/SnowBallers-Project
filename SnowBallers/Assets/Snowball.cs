using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit!");
            collision.gameObject.GetComponent<Health>().decreaseHealth(1);
            collision.gameObject.GetComponent<ChangeColor>().ChangeObjectColor();
        }
    }
}
