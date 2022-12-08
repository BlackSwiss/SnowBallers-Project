using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineMenu : MonoBehaviour
{
    public void OnClickHost()
    {
        UIManager.OpenMenu(Menu.LOBBY_MENU, gameObject);
    }

    public void OnClickConnect()
    {
        UIManager.OpenMenu(Menu.LOBBY_MENU, gameObject);
    }

    public void OnClickBack()
    {
        UIManager.OpenMenu(Menu.MAIN_MENU, gameObject);
    }
}
