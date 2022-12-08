using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PS4Controller : MonoBehaviour
{
    public Gamepad gamepad;
    public GameObject player;
    public GameObject leftController;
    public GameObject rightController;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            if(Gamepad.all[i].name == "DualShock4GamepadHID")
            {
                Debug.Log("PS4 Controller detected");
                gamepad = Gamepad.all[i];
                return;
            }
        }
        Debug.Log("No PS4 Controller is plugged in");
    }

    // Update is called once per frame
    void Update()
    {
        if(gamepad != null && player != null)
        {
            //Movement
            if(gamepad.leftStick.left.IsPressed())
            {
                player.transform.Translate(Vector3.left * Time.deltaTime * 5f);
            }
            else if(gamepad.leftStick.right.IsPressed())
            {
                player.transform.Translate(Vector3.right * Time.deltaTime * 5f);
            }
            else if(gamepad.leftStick.up.IsPressed())
            {
                player.transform.Translate(Vector3.forward * Time.deltaTime * 5f);
            }
            else if(gamepad.leftStick.down.IsPressed())
            {
                player.transform.Translate(Vector3.back * Time.deltaTime * 5f);
            }
            else if(gamepad.leftStickButton.IsPressed())
            {
                player.transform.position = new Vector3(0,1.25f,-2);
            }


            //Rotation
            if(gamepad.rightStick.left.IsPressed())
            {
                player.transform.Rotate(Vector3.down * Time.deltaTime * 100f, Space.World);
            }
            else if(gamepad.rightStick.right.IsPressed())
            {
                player.transform.Rotate(Vector3.up * Time.deltaTime * 100f, Space.World);
            }
            else if(gamepad.rightStick.up.IsPressed())
            {
                player.transform.Rotate(Vector3.left * Time.deltaTime * 100f, Space.World);
            }
            else if(gamepad.rightStick.down.IsPressed())
            {
                player.transform.Rotate(Vector3.right * Time.deltaTime * 100f, Space.World);
            }
            else if(gamepad.rightStickButton.IsPressed())
            {
                player.transform.rotation = new Quaternion(0,0,0,0);
            }


            //Interaction
            else if(gamepad.rightTrigger.IsPressed())
            {
                //InputDevice left = leftController.GetComponent<XRController>();
                //left.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            }
            else if(gamepad.leftTrigger.IsPressed())
            {
                //InputDevice right = rightController.GetComponent<XRController>();
                //left.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
            }
            else
            {
                return;
            }
        }
    }
}
