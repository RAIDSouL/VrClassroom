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

            handFx.SetActive(true);

        }
        else
        {
            targetTranform = Normal;
            { handFx.SetActive(false); }
        }

        Controller.position = targetTranform.position;
        Controller.rotation = targetTranform.rotation;
    }
}
