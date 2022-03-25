using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviourPunCallbacks
{
    public CanvasManager VrCanvas, AndroidCanvas;
    protected virtual void Start()
    {
        switch (PlatformSetting.Instance.platform)
        {
            case Platform.VR:
                VrCanvas.gameObject.SetActive(true);
                AndroidCanvas.gameObject.SetActive(false);
                break;
            case Platform.ANDROID:
                AndroidCanvas.gameObject.SetActive(true);
                VrCanvas.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

}
