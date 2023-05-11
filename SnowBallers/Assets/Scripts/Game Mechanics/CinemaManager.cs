using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class CinemaManager : MonoBehaviourPun
{
    public GameObject mainCamera;
    public GameObject cinemaCamera;
    public GameObject povCamera;
    public GameObject intro;
    public GameObject outro;
    public bool isMovementActive = true;
    private bool cinemaTracking = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        syncMainCamera();
    }

    public void syncPOVCamera(Transform target)
    {
        povCamera.transform.position = target.position;
        povCamera.transform.rotation = target.rotation;
    }

    public void syncMainCamera()
    {
        if(cinemaTracking)
        {
            mainCamera.transform.position = cinemaCamera.transform.position;
            mainCamera.transform.rotation = cinemaCamera.transform.rotation;
        }
    }

    [PunRPC]
    public void playIntroCutscene()
    {   
        toggleMovement();
        cinemaTracking = true;
        cinemaCamera.SetActive(true);
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
        cinemaCamera.SetActive(true);
        outro.SetActive(true);
        //Disable cutscene assets once outro is finished.
        Invoke(nameof(disableAllCinema),4.5f);
    }

    private void disableAllCinema()
    {
        cinemaTracking = false;
        cinemaCamera.SetActive(false);
        intro.SetActive(false);
        outro.SetActive(false);
    }

    private void toggleMovement()
    {
        isMovementActive = !isMovementActive;
        GameObject locomotion = GameObject.Find("Locomotion System");
        ActionBasedContinuousMoveProvider moveProvider = locomotion.GetComponentInChildren<ActionBasedContinuousMoveProvider>();
        moveProvider.enabled = !moveProvider.enabled;
    }
}
