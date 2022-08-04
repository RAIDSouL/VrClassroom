using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentScreenAndroid : MonoBehaviour
{
    bool isActive = false;

    private void Start()
    {
        if (PlatformSetting.Instance.platform != Platform.ANDROID)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<RawImage>().material = GameObject.Find("TeacherScreen").GetComponent<MeshRenderer>().material;
    }
    public void SetObject()
    {
        isActive = !isActive;
        if (isActive) 
        {
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-423, 423, 0);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(940, 700);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-160,160, 0);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(235, 175);
        }
    }
}
