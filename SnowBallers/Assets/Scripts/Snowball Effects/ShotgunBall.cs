using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class ShotgunBall : XRGrabNetworkInteractable
{
    [Header("Shotgun attributes")]
    [SerializeField]
    GameObject pellets;
    [SerializeField]
    int numOfPellets;

    [SerializeField]
    float strayFactor;

    float randomNumberX;
    float randomNumberY;

    GameObject newPellet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        //Hide 3ball snowball (default)
        foreach(MeshRenderer mesh in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
        //Spawn new pellets
        for (int i = 0; i < numOfPellets; i++)
        {
            newPellet = PhotonNetwork.Instantiate(pellets.name, transform.position, Quaternion.identity);
            randomNumberX = Random.Range(-strayFactor, strayFactor);
            randomNumberY = Random.Range(-strayFactor, strayFactor);

            newPellet.transform.Rotate(randomNumberX, randomNumberY, 0);
        }
        base.OnSelectExited(interactor);
    }
}
