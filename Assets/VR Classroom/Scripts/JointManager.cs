using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManager : MonoBehaviour
{
    public Transform Normal, Handup, Controller;
    public GameObject handFx;
    GameObject FxOn;
    bool isOn;
    float rotateValue;
    public void OnHandup(bool up)
    {
        Transform targetTranform = null;
        isOn = up;
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
    private void Update()
    {
        if (isOn) 
        {
            rotateValue += Time.deltaTime*72;
            if (rotateValue > 360) rotateValue = 0;
            handFx.transform.localEulerAngles = new Vector3(0, rotateValue, 0);
        }
    }
}
