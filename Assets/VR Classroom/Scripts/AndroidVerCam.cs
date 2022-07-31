using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndroidVerCam : MonoBehaviour
{
    [SerializeField] GameObject cc;
    Vector2 down,delta;
    Vector3 save;
    bool hold;
    void Start()
    {
        if (PlatformSetting.Instance.platform != Platform.ANDROID)
        {
            this.enabled = false;
        }
        save = Vector3.zero;
    }

    void Update()
    {      
        if (Mouse.current.leftButton.isPressed)
        {
            if (!hold) down = Mouse.current.position.ReadValue();
            hold = true;
            delta = down - Mouse.current.position.ReadValue();
            cc.transform.localEulerAngles = save +  new Vector3(delta.y/20, delta.x/20, 0);
        }
        else 
        {
            save = cc.transform.localEulerAngles;
            hold = false;
        }     
    }  
}
