using ChiliGames.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Handup : MonoBehaviour
{
    public void Up(Toggle toggle)
    {
        if (PlatformManager.instance.MyChar == null)
            return;

        object[] data = new object[] { PlatformManager.instance.MyChar.sit, toggle.isOn };

        Hashtable propertiesToSet = new Hashtable();
        propertiesToSet.Add(PropertiesKey.Handup, data);

        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);

        //PlatformManager.instance.MyChar.JointManager.OnHandup(toggle.isOn);
    }
}
