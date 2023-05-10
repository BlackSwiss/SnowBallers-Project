using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

public class XRGrabbableObject : XRBaseInteractable
{
    [SerializeField]
    Collider ownerCollider;
    [SerializeField]
    private GameObject[] grabbableObject;

    public int randomInt;
    GameObject currentSnowball;

    [SerializeField]
    private Transform transformToInstantiate;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        randomInt = Random.Range(0, grabbableObject.Length);
        currentSnowball = grabbableObject[randomInt];
        // Instantiate object
        GameObject newObject = PhotonNetwork.Instantiate(currentSnowball.name, transformToInstantiate.position, Quaternion.identity);

        //Grab snowball owner collider so we dont hit self, and pass to snowball
        Snowball snowballScript = newObject.GetComponent<Snowball>();

        snowballScript.owner = ownerCollider;
        Physics.IgnoreCollision(ownerCollider, newObject.GetComponent<Collider>());
        // Get grab interactable from prefab
        XRGrabInteractable objectInteractable = newObject.GetComponent<XRGrabInteractable>();

        // Select object into same interactor
        interactionManager.SelectEnter(args.interactorObject, objectInteractable);

        base.OnSelectEntered(args);
    }

    
}
