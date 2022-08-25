using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentScreen : MonoBehaviour
{
    [SerializeField] GameObject[] slideObj;//0 slide  1 board
    int activeScreen = 3;
    int currentFrame = 0;
    Material teacherScreen, selfMat;
    PhotonView pv;
    private void Start()
    {
        pv = gameObject.transform.parent.transform.parent.GetComponentInChildren<PhotonView>();
        if (PlatformSetting.Instance.platform == Platform.ANDROID || (!pv.IsMine && pv != null))
        {
            Destroy(slideObj[1].gameObject);
            Destroy(slideObj[0].gameObject);
            return;
        }
        else
        {
            //  print(gameObject.transform.parent.GetComponentInChildren<PhotonView>());
        }
        selfMat = gameObject.GetComponent<MeshRenderer>().material;
    }
    public void SetObject(int ID)
    {       
        if (ID != activeScreen)
        {
            hideAll();
            slideObj[ID].gameObject.transform.localPosition = new Vector3(.31f, .88f, .5f);
            slideObj[ID].gameObject.transform.localScale = new Vector3(.5f, .32f, .8f);
            if(ID==1)
            slideObj[ID].gameObject.transform.localScale = new Vector3(-.5f, -.32f, .8f);
            activeScreen = ID;
        }
        else
        {
            hideAll();
            activeScreen = 3;
        }
    }
    void hideAll()
    {
        slideObj[0].gameObject.transform.localPosition = new Vector3(.31f, .85f, .55f);
        slideObj[0].gameObject.transform.localScale = new Vector3(.25f, .16f, .8f);

        slideObj[1].gameObject.transform.localPosition = new Vector3(.31f, 1.03f, .55f);
        slideObj[1].gameObject.transform.localScale = new Vector3(-.25f, -.16f, .8f);
    }
    private void Update()
    {
        print(activeScreen);
        currentFrame++;
        if (currentFrame % 2 != 0)
        {
            return;
        }
        slideObj[0].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("TeacherScreen").GetComponent<MeshRenderer>().material.mainTexture;
        slideObj[1].gameObject.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("Whiteboard R").GetComponent<MeshRenderer>().material.mainTexture;
    }
}
