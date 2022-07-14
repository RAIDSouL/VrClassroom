using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManager : MonoBehaviour
{
    public Transform Normal, Handup, Controller;
    public GameObject handFx;
    GameObject FxOn;
    public void OnHandup(bool up)
    {
        Transform targetTranform = null;
        if (up)
        {
            targetTranform = Handup;
            FxOn = Instantiate(handFx, Handup);
            FxOn.transform.position = Handup.transform.position;
        }
        else
        {
            targetTranform = Normal;
            if (FxOn != null) { Destroy(FxOn); }
        }

        Controller.position = targetTranform.position;
        Controller.rotation = targetTranform.rotation;
    }
}
