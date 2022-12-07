using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;

    public Health(float health) {
        this.health = health;
    }

    public void setHealth(float health) {
        this.health = health;        
    }

    public float getHealth() {
        return health;
    }

    public void decreaseHealth(float health) {
        this.health -= health;
    }
}