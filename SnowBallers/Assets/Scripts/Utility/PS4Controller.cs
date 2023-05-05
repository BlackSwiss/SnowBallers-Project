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
    public LayerMask terrainLayer;
    public LayerToggleManager hudManager;
    public LayerToggleManager menuManager;
    private float minY;

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

        minY = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(gamepad != null && player != null)
        {
            //Movement
            if(gamepad.leftStick.left.IsPressed())
            {
                player.transform.Translate(Vector3.left * Time.deltaTime * 5f, Space.Self);
            }
            else if(gamepad.leftStick.right.IsPressed())
            {
                player.transform.Translate(Vector3.right * Time.deltaTime * 5f, Space.Self);
            }
            else if(gamepad.leftStick.up.IsPressed())
            {
                player.transform.Translate(Vector3.forward * Time.deltaTime * 5f, Space.Self);
            }
            else if(gamepad.leftStick.down.IsPressed())
            {
                player.transform.Translate(Vector3.back * Time.deltaTime * 5f, Space.Self);
            }


            //Stop player from going below initial Y position
            Ray ray = new Ray(player.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
            {
                minY = hit.point.y + 1.25f;
            }
            player.transform.position = new Vector3(player.transform.position.x, minY, player.transform.position.z);


            //Rotation
            if(gamepad.rightStick.left.IsPressed())
            {
                player.transform.Rotate(Vector3.down * Time.deltaTime * 150f, Space.World);
            }
            else if(gamepad.rightStick.right.IsPressed())
            {
                player.transform.Rotate(Vector3.up * Time.deltaTime * 150f, Space.World);
            }
            else if(gamepad.rightStick.up.IsPressed())
            {
                player.transform.Rotate(Vector3.left * Time.deltaTime * 150f, Space.Self);
            }
            else if(gamepad.rightStick.down.IsPressed())
            {
                player.transform.Rotate(Vector3.right * Time.deltaTime * 150f, Space.Self);
            }
            else if(gamepad.rightStickButton.IsPressed())
            {
                player.transform.rotation = new Quaternion(0,0,0,0);
            }


            //Interaction
            if(gamepad.rightTrigger.IsPressed())
            {
                //InputDevice left = leftController.GetComponent<XRController>();
                //left.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            }
            else if(gamepad.leftTrigger.IsPressed())
            {
                //InputDevice right = rightController.GetComponent<XRController>();
                //left.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
            }

            //Hud
            if(gamepad.selectButton.IsPressed())
            {
                if(hudManager != null)
                {
                    hudManager.toggleLayer();
                }
            }
            //Menu
            else if(gamepad.startButton.IsPressed())
            {
                if(menuManager != null)
                {
                    menuManager.toggleLayer();
                }
            }


            else
            {
                return;
            }
        }
    }
}
