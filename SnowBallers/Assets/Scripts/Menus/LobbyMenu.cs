using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    public void OnClickBack()
    {
        UIManager.OpenMenu(Menu.MAIN_MENU, gameObject);
    }
    
}
