using ChiliGames.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handup : MonoBehaviour
{
    public void Up(Toggle toggle)
    {
        if (PlatformManager.instance.MyChar == null)
            return;

        PlatformManager.instance.MyChar.JointManager.OnHandup(toggle.isOn);
    }
}
