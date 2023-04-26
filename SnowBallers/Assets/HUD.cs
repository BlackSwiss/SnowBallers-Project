using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HUD : MonoBehaviour
{
    public Camera camera;
    public GameObject canvas;
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    private bool currentCanvasStatus = false;
    private bool buttonRecentlyPressed = false;
    private float resetButtonDelay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if(buttonRecentlyPressed)
                return;

            if(targetDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonPressed))
            {
                if(menuButtonPressed)
                {
                    Debug.Log("Toggling HUD");
                    currentCanvasStatus = !currentCanvasStatus;
                    buttonRecentlyPressed = true;
                    Invoke(nameof(resetButton), resetButtonDelay);

                    if(currentCanvasStatus)
                        camera.cullingMask = 2048;
                    else
                        camera.cullingMask = 4096;
                }
            }
        }
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            Debug.Log("Controller left found!");
            targetDevice = devices[0];
        }
    }
    
    void resetButton()
    {
        buttonRecentlyPressed = false;
    }
}
