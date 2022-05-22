using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManager : MonoBehaviour
{
    public Transform Normal, Handup, Controller;

    public void OnHandup(bool up)
    {
        Transform targetTranform = null;
        if (up)
        {
            targetTranform = Handup;
        }
        else
        {
            targetTranform = Normal;
        }

        Controller.position = targetTranform.position;
        Controller.rotation = targetTranform.rotation;
    }
}
