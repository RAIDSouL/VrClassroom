using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice;
using ChiliGames.VRClassroom;
using Photon.Pun;
using Photon.Voice.Unity;

public class MuteControl : MonoBehaviour
{
    [SerializeField] Image BtnIcon;
    bool ismute;
    GameObject self;
   public void MuteToggle()
    {
        if (self == null)
        {
            self = PlatformManager.instance.MyChar.gameObject;
            ismute = self.GetComponent<Recorder>().IsRecording;
        }
        ismute = !ismute;
        if (ismute)
        {
            BtnIcon.color = Color.red;
        }
        else
        {
            BtnIcon.color = Color.white;
        }
        self.GetComponent<Recorder>().IsRecording = !ismute;

    }
}
