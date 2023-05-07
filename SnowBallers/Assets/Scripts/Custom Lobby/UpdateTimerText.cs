using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UpdateTimerText : MonoBehaviour
{
    private NetworkPlayerSpawner networkPlayerSpawner;
    private double time;

    // Start is called before the first frame update
    void Start()
    {
        networkPlayerSpawner = GameObject.Find("Network Manager").GetComponent<NetworkPlayerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTimerText();
    }

    public void updateTimerText()
    {
        time = networkPlayerSpawner.getStartTime();
        gameObject.GetComponent<TextMeshProUGUI>().text = time.ToString();
    }
}
