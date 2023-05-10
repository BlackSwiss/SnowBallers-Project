using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LayerToggleManager : MonoBehaviour
{
    public Camera camera;
    public InputDeviceCharacteristics controllerCharacteristics;
    public LayerMask viewableLayers;
    public LayerMask ignoreLayers;
    public GameObject[] stuffToToggle;
    public GameObject customLobbyUI;
    private InputDevice targetDevice;
    private bool buttonRecentlyPressed = false;
    private float resetButtonDelay = 0.25f;

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
            if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool menuButtonPressed))
            {
                if(menuButtonPressed)
                {
                    toggleLayer();
                }
            }
        }
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
            targetDevice = devices[0];
    }
    
    void resetButton()
    {
        buttonRecentlyPressed = false;
    }

    public void toggleLayer()
    {
        if(buttonRecentlyPressed)
            return;

        if(customLobbyUI.activeInHierarchy)
            return;

        buttonRecentlyPressed = true;
        Invoke(nameof(resetButton), resetButtonDelay);

        if(camera.cullingMask != viewableLayers)
        {
            camera.cullingMask = viewableLayers;
            foreach(GameObject gameObject in stuffToToggle)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            camera.cullingMask = ignoreLayers;
            foreach(GameObject gameObject in stuffToToggle)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
