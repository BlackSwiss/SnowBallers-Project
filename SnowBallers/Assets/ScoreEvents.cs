using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreEvents : MonoBehaviour
{
    public static ScoreEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> onPlayerHit;

    public void playerHit(int ownerID)
    {
        if(onPlayerHit != null)
        {
            onPlayerHit(ownerID);
        }
    }
}
