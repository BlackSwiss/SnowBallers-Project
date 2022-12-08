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

    void decreaseHealth(float amount) {
        currentHealth -= amount;
        //run animation for damage
    }
}