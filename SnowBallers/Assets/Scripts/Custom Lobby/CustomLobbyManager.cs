using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLobbyManager : MonoBehaviour
{
    public Camera camera;
    public LayerMask viewableLayers;
    public LayerMask ignoreLayers;
    public GameObject[] stuffToEnable;
    public GameObject[] stuffToDisable;
    public GameObject customLobbyUI;
    
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
            foreach(GameObject gameObject in stuffToDisable)
            {
                gameObject.SetActive(false);
            }
            foreach(GameObject gameObject in stuffToEnable)
            {
                gameObject.SetActive(true);
                gameObject.layer = 15;
            }
        }
        else
        {
            camera.cullingMask = ignoreLayers;
            foreach(GameObject gameObject in stuffToEnable)
            {
                gameObject.SetActive(false);
                gameObject.layer = 14;
            }
            foreach(GameObject gameObject in stuffToDisable)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
