using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabbableObject : XRBaseInteractable
{
    [SerializeField]
    private GameObject grabbableObject;

    [SerializeField]
    private Transform transformToInstantiate;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Instantiate object
        GameObject newObject = Instantiate(grabbableObject, transformToInstantiate.position, Quaternion.identity);

        // Get grab interactable from prefab
        XRGrabInteractable objectInteractable = newObject.GetComponent<XRGrabInteractable>();

        // Select object into same interactor
        interactionManager.SelectEnter(args.interactorObject, objectInteractable);

        base.OnSelectEntered(args);
    }
}
