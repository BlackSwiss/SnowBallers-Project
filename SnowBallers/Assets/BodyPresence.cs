using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BodyPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject bodyModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedBodyModel;
    private Animator bodyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            spawnedBodyModel = Instantiate(bodyModelPrefab);
            bodyAnimator = spawnedBodyModel.GetComponent<Animator>();
        }
    }

    void UpdateBodyAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue))
        {
            if(!axisValue.Equals(Vector2.zero))
            {
                bodyAnimator.SetBool("Move", true);
            }
            else
            {
                bodyAnimator.SetBool("Move", false);
            }
        }
        else
        {
            bodyAnimator.SetBool("Move", false);
        }
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
            if (spawnedBodyModel)
            {
                spawnedBodyModel.SetActive(true);
            }
            UpdateBodyAnimation();
        }
    }
}
