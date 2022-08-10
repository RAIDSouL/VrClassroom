using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AndroidVerCam : MonoBehaviour
{
    [SerializeField] GameObject cc;
    Vector2 down, delta;
    Vector3 save;
    bool hold;
    TouchControl touchcontrol;
    void Awake()
    {
        touchcontrol = new TouchControl();
    }
    private void OnEnable()
    {
        touchcontrol.Enable();
    }
    private void OnDisable()
    {
        touchcontrol.Disable();
    }
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

        if (touchcontrol.Touch.TouchDrag.phase == InputActionPhase.Performed)
        {
            if (!hold) down = touchcontrol.Touch.TouchInput.ReadValue<Vector2>();
            hold = true;
            delta = down - touchcontrol.Touch.TouchInput.ReadValue<Vector2>();
            cc.transform.localEulerAngles = save + new Vector3(delta.y / 20, delta.x / 20, 0);
        }

        else
        {
            save = cc.transform.localEulerAngles;
            hold = false;
        }



        //   if (Mouse.current.leftButton.isPressed)
        //   {
        //       if (!hold) down = Mouse.current.position.ReadValue();
        //       hold = true;
        //       delta = down - Mouse.current.position.ReadValue();
        //       cc.transform.localEulerAngles = save + new Vector3(delta.y / 20, delta.x / 20, 0);
        //   }
        //   else
        //   {
        //       save = cc.transform.localEulerAngles;
        //       hold = false;
        //   }
    }
}
