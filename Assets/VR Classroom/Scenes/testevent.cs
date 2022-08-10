using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class testevent : MonoBehaviour
{
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

    private void Start()
    {
     //  touchcontrol.Touch.TouchDrag.started += ContextMenu => startTouch(ContextMenu);  
     //  touchcontrol.Touch.TouchDrag.canceled += ContextMenu => endTouch(ContextMenu);
       
    }

    private void endTouch(InputAction.CallbackContext context)
    {
        //   throw new NotImplementedException();
        Debug.Log("end - " + touchcontrol.Touch.TouchDrag.ReadValue<Vector2>());
    }

    private void startTouch(InputAction.CallbackContext context)
    {
        Debug.Log("start - " + touchcontrol.Touch.TouchDrag.ReadValue<Vector2>());
    //    throw new NotImplementedException();
      
    }

    private void Update()
    {

        if (touchcontrol.Touch.TouchDrag.phase == InputActionPhase.Performed)
        {
            print(touchcontrol.Touch.TouchInput.ReadValue<Vector2>());
        }
        else if (touchcontrol.Touch.TouchDrag.phase == InputActionPhase.Waiting) { print("aaa"); }
    }
}
