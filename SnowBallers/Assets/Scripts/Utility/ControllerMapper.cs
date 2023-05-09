using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerMapper : MonoBehaviour
{
    public Gamepad gamepad;
    public GameObject player;
    public float moveSpeed = 3.75f;
    public float turnSpeed = 100f;
    public LayerMask terrainLayer;
    public LayerToggleManager hudManager;
    public LayerToggleManager menuManager;
    private float initialY;
    private float minimumY;
    private bool setInitialY = false;
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Disconnected:
                    Debug.Log("Controller disconnected");
                    gamepad = null;
                    break;
                default:
                    break;
            }
        };

        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gamepad == null)
        {
            TryInitializeGamepad();
            return;
        }
        else if(player != null)
        {
            //If game over, stop player from moving.
            if(scoreManager == null || !scoreManager.gameOver)
            {
                //Movement - left stick
                if(gamepad.leftStick.left.IsPressed())
                {
                    player.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed, Space.Self);
                }
                else if(gamepad.leftStick.right.IsPressed())
                {
                    player.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.Self);
                }
                else if(gamepad.leftStick.up.IsPressed())
                {
                    player.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);
                }
                else if(gamepad.leftStick.down.IsPressed())
                {
                    player.transform.Translate(Vector3.back * Time.deltaTime * moveSpeed, Space.Self);
                }
            }


            //Stop player from going below initial Y position
            Ray ray = new Ray(player.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
            {
                if(!setInitialY)
                {
                    initialY = hit.distance;
                    setInitialY = true;
                }
                minimumY = hit.point.y + initialY;
            }
            player.transform.position = new Vector3(player.transform.position.x, minimumY, player.transform.position.z);


            //Rotation - right stick
            if(gamepad.rightStick.left.IsPressed())
            {
                player.transform.Rotate(Vector3.down * Time.deltaTime * turnSpeed, Space.World);
            }
            else if(gamepad.rightStick.right.IsPressed())
            {
                player.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed, Space.World);
            }
            else if(gamepad.rightStick.up.IsPressed())
            {
                player.transform.Rotate(Vector3.left * Time.deltaTime * turnSpeed, Space.Self);
            }
            else if(gamepad.rightStick.down.IsPressed())
            {
                player.transform.Rotate(Vector3.right * Time.deltaTime * turnSpeed, Space.Self);
            }
            else if(gamepad.rightStickButton.IsPressed())
            {
                player.transform.rotation = new Quaternion(0,0,0,0);
            }


            //Interaction
            //Used XR Device Simulator Input Actions to map trigger buttons
            //Left trigger - must be held down to use right trigger
            //Right trigger - used to click UI buttons


            //Hud - select button
            if(gamepad.selectButton.IsPressed())
            {
                if(hudManager != null)
                {
                    hudManager.toggleLayer();
                }
            }


            //Menu - start button
            else if(gamepad.startButton.IsPressed())
            {
                if(menuManager != null)
                {
                    menuManager.toggleLayer();
                }
            }
        }
    }

    private void TryInitializeGamepad()
    {
        gamepad = Gamepad.current;
        if(gamepad != null)
        {
            Debug.Log("Controller connected");
        }
    }
}
