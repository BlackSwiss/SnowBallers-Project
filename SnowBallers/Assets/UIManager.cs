using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    public static bool IsInitialized { get; private set; }
    public static GameObject mainMenu, onlineMenu, optionsMenu, lobbyMenu;

    public static void Init()
    {
        GameObject canvas = GameObject.Find("MainUI");
        mainMenu = canvas.transform.Find("MainMenu").gameObject;
        onlineMenu = canvas.transform.Find("OnlineMenu").gameObject;
        optionsMenu = canvas.transform.Find("OptionsMenu").gameObject;
        lobbyMenu = canvas.transform.Find("LobbyMenu").gameObject;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu)
    {
        Init();

        switch(menu)
        {
            case Menu.MAIN_MENU:
                mainMenu.SetActive(true);
                break;
            case Menu.ONLINE_MENU:
                onlineMenu.SetActive(true);
                break;
            case Menu.OPTIONS_MENU:
                optionsMenu.SetActive(true);
                break;
            case Menu.LOBBY_MENU:
                lobbyMenu.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
    }
}
