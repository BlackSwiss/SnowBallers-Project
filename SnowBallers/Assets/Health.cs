using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;

    public Animator animate;

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    public void decreaseHealth(float amount) {
        //Old Healh decrease code
        //currentHealth -= amount;
        //Debug.Log("Current player health: " + currentHealth);
        //run animation for damage

        //New Health with just score code

    }


    public void hitAnimation()
    {
        animate.SetBool("isHit", true);
    }

    public void stopHitAnimation()
    {
        animate.SetBool("isHit", false);
    }
}