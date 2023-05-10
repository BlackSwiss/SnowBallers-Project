using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLobbyManager : MonoBehaviour
{
    public Camera camera;
    public LayerMask viewableLayers;
    public LayerMask ignoreLayers;
    public GameObject customLobbyUI;
    public GameObject rightHandDirect;
    public GameObject rightHandRay;

    public void enableCustomLobbyUI()
    {
        customLobbyUI.SetActive(true);
        toggleLayer();
    }

    public void disableCustomLobbyUI()
    {
        customLobbyUI.SetActive(false);
        toggleLayer();
    }

    public void toggleLayer()
    {
        if(camera.cullingMask != viewableLayers)
        {
            camera.cullingMask = viewableLayers;
            rightHandDirect.SetActive(false);
            rightHandRay.SetActive(true);
            rightHandRay.layer = 15;
        }
        else
        {
            camera.cullingMask = ignoreLayers;
            rightHandRay.SetActive(false);
            rightHandDirect.SetActive(true);
            rightHandRay.layer = 14;
        }
    }
}
