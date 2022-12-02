using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClickOnline()
    {
        UIManager.OpenMenu(Menu.ONLINE_MENU, gameObject);
    }

    public void OnClickOptions()
    {
        UIManager.OpenMenu(Menu.OPTIONS_MENU, gameObject);
    }
}
