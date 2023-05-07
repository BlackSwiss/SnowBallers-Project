using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLobbyManager : MonoBehaviour
{
    public Camera camera;
    public LayerMask viewableLayers;
    public LayerMask ignoreLayers;
    public GameObject customLobbyUI;
    public GameObject controller;

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
            controller.layer = 15;
        }
        else
        {
            camera.cullingMask = ignoreLayers;
            controller.layer = 14;
        }
    }
}
