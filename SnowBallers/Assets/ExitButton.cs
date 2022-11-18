using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    Button button;

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Exiting");
    }
}
