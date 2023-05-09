using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject cinemaCamera;
    private bool cinemaTracking = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.transform.position = cinemaCamera.transform.position;
        mainCamera.transform.rotation = cinemaCamera.transform.rotation;
        Invoke(nameof(disableCinematicTracking),9);
        Invoke(nameof(enableCinematicTracking),65);
        Invoke(nameof(disableCinematicTracking),70);
    }

    // Update is called once per frame
    void Update()
    {
        if(cinemaTracking)
        {
            mainCamera.transform.position = cinemaCamera.transform.position;
            mainCamera.transform.rotation = cinemaCamera.transform.rotation;
        }
    }

    private void enableCinematicTracking()
    {
        cinemaTracking = true;
        cinemaCamera.SetActive(true);
    }
    private void disableCinematicTracking()
    {
        cinemaTracking = false;
        cinemaCamera.SetActive(false);
    }
}
