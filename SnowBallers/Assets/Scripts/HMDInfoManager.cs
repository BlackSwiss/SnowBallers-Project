using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    //TURN THIS OBJECT ON IF YOU WANT KEYBOARD CONTORLS IN THIS SCENE
    public GameObject HMDSimulator;
    void Start()
    {
        Debug.Log("Is device active: " + XRSettings.isDeviceActive);
        Debug.Log("Device name is: " + XRSettings.loadedDeviceName);

        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No headset Plugged in");
        }
        else if (XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "MockHMDDisplay")
        {
            Debug.Log("Using mock hmd");
        }
        else
        {
            Debug.Log("We have a headset " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Turned on Keyboard Controls!");
            turnonKeyboardControls();
        }
    }

    void turnonKeyboardControls()
    {
        HMDSimulator.SetActive(true);
    }
}
