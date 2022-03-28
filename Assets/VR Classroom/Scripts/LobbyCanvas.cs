using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyCanvas : CanvasManager
{
    public GameObject LoginGroup, JoinGroup;

    public TMP_InputField UsernameInput, RoomnameInput;

    public TMP_Dropdown UserType;

    public static LobbyCanvas instance;

    public override void SetInstance(bool t)
    {
        gameObject.SetActive(t);
        if (t)
            instance = this;
    }

    public void OnLogin()
    {
        LoginGroup.SetActive(false);
        JoinGroup.SetActive(true);
        NetworkManager.instance.Connect();
    }

    public void JoinRoomClick()
    {
        NetworkManager.instance.ConnectToRoom();
    }
}
