using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentScreen : MonoBehaviour
{
    bool isActive = false;
    Material teacherScreen, selfMat;
    PhotonView pv;
    private void Start()
    {
        pv = gameObject.transform.parent.transform.parent.GetComponentInChildren<PhotonView>();
        if (PlatformSetting.Instance.platform == Platform.ANDROID || (!pv.IsMine && pv != null))
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            print(gameObject.transform.parent.GetComponentInChildren<PhotonView>());

        }
        selfMat = gameObject.GetComponent<MeshRenderer>().material;
    }
    public void SetObject()
    {
        isActive = !isActive;
        if (isActive) //85 88
        {
            gameObject.transform.localPosition = new Vector3(.31f, .88f, .5f);
            gameObject.transform.localScale = new Vector3(.5f, .32f, .8f);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(.31f, .85f, .5f);
            gameObject.transform.localScale = new Vector3(.25f, .16f, .8f);
        }
    }
    private void Update()
    {
        gameObject.GetComponent<MeshRenderer>().material = GameObject.Find("TeacherScreen").GetComponent<MeshRenderer>().material;
    }
}
