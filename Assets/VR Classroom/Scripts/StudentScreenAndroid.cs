using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentScreenAndroid : MonoBehaviour
{
    //  bool isActive = false;
    [SerializeField] GameObject[] miniScreen;
 
    int activeScreen = 3;
    private void Start()
    {
        if (PlatformSetting.Instance.platform != Platform.ANDROID)
        {
            Destroy(miniScreen[1].gameObject);
            Destroy(miniScreen[0].gameObject);
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        print(miniScreen[0].gameObject.GetComponent<RectTransform>().anchoredPosition);
        miniScreen[0].gameObject.GetComponent<RawImage>().texture = GameObject.Find("TeacherScreen").GetComponent<MeshRenderer>().material.mainTexture;
        miniScreen[1].gameObject.GetComponent<RawImage>().texture = GameObject.Find("Whiteboard R").GetComponent<MeshRenderer>().material.mainTexture;
    }
    public void SetObject(int ID)
    {

        if (activeScreen != ID)
        {
            closeAll();
            miniScreen[ID].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-475, 150,-10);
            miniScreen[ID].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1175, 875);
            activeScreen = ID;
        }
        else
        {
            closeAll();
            activeScreen = 3;
        }
    }
    void closeAll()
    {
        miniScreen[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-45, 75, 0);
        miniScreen[0].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(235, 175);
        miniScreen[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-45, 275, 0);
        miniScreen[1].gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(235, 175);
    }
}
