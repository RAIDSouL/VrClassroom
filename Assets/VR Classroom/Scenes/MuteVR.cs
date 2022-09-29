using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiliGames.VRClassroom;
using Photon.Voice.Unity;
using Unity.VisualScripting;

public class MuteVR : MonoBehaviour
{
    bool ismute;
    GameObject self;
    [SerializeField] Material muteMats;

    private void OnTriggerEnter(Collider other)
    {
        MuteToggle();
    }
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
            muteMats.color = Color.red;
        }
        else
        {
            muteMats.color = Color.white;
        }
        self.GetComponent<Recorder>().IsRecording = !ismute;

    }

}
