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

    public event Action<int,int> onPlayerHit;

    public void playerHit(int ownerID, int scoreAmount)
    {
        if(onPlayerHit != null)
        {
            onPlayerHit(ownerID, scoreAmount);
        }
    }

    public event Action<int> onHeadshot;

    public void headshot(int ownerID)
    {
        if(onHeadshot != null)
        {
            onHeadshot(ownerID);
        }
    }
}
