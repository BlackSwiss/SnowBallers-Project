using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject timerText;
    double time;

    // Start is called before the first frame update
    void Start()
    {
        timerText.GetComponent<TextMeshProUGUI>().text = "Timer : ";
    }

    // Update is called once per frame
    void Update()
    {
        time = GameObject.Find("Network Manager").GetComponent<NetworkPlayerSpawner>().getTimerDecrement();
        if(time >= 0)
        {
            timerText.GetComponent<TextMeshProUGUI>().text = "Timer: " + time.ToString("#.00");
        }
        else
        {
            timerText.GetComponent<TextMeshProUGUI>().text = "Timer: TIME UP!";
        }
    }
}
