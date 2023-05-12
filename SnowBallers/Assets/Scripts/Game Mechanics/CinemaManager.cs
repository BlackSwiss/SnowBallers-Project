using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class CinemaManager : MonoBehaviourPun
{
    public GameObject povCamera;
    public GameObject intro;
    public GameObject outro;
    public bool isMovementActive = true;
    private bool cinemaTracking = false;
    private GameObject rig;
    private ActionBasedContinuousMoveProvider moveProvider;

    // Start is called before the first frame update
    void Start()
    {
        rig = GameObject.Find("XR Origin");
        moveProvider = GameObject.Find("Locomotion System").GetComponentInChildren<ActionBasedContinuousMoveProvider>();
    }

    public void syncPOVCamera(Transform target)
    {
        povCamera.transform.position = target.position;
        povCamera.transform.rotation = target.rotation;
    }

    [PunRPC]
    public void playIntroCutscene()
    {   
        toggleMovement();
        cinemaTracking = true;
        intro.SetActive(true);
        //Disable cutscene assets once intro is finished.
        Invoke(nameof(disableAllCinema),9);
        Invoke(nameof(toggleMovement),9);
    }

    [PunRPC]
    public void playOutroCutscene()
    {
        //Delay outro cutscene until game over sound effect is finished.
        Invoke(nameof(playOutroCutsceneInvoke),5);
    }

    private void playOutroCutsceneInvoke()
    {
        toggleMovement();
        cinemaTracking = true;
        outro.SetActive(true);
        //Disable cutscene assets once outro is finished.
        Invoke(nameof(disableAllCinema),4.5f);
    }

    private void disableAllCinema()
    {
        cinemaTracking = false;
        intro.SetActive(false);
        outro.SetActive(false);
        rig.transform.rotation = povCamera.transform.rotation;
    }

    private void toggleMovement()
    {
        isMovementActive = !isMovementActive;
        moveProvider.enabled = isMovementActive;
    }
}
