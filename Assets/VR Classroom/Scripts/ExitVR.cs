using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using ChiliGames.VRClassroom;

public class ExitVR : MonoBehaviour
{
    bool hold;
    float time;
    PhotonView pv;
    public float cooldown;
    bool ready = true;
    [SerializeField] Material mt;
   float colorT;

    private void Start()
    {
        pv = gameObject.transform.parent.transform.parent.GetComponentInChildren<PhotonView>();
        if (PlatformSetting.Instance.platform == Platform.ANDROID || (!pv.IsMine && pv != null))
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!ready) return;
        hold = true;

        if (cooldown != 0)
            StartCoroutine(WaitCooldown());
    }
    IEnumerator WaitCooldown()
    {
        ready = false;
        yield return new WaitForSeconds(cooldown);
        ready = true;
    }
    private void OnTriggerExit(Collider other)
    {
        hold = false;
        time = 0;
    }
   

    void Update()
    {
        if (hold)
        {
            time += Time.deltaTime;
            colorT = Mathf.Clamp(time / 5, .5f, 1);
            mt.color = new Color(1, .24f, .24f, colorT);
            if (time > 3)
            { _ExitVR(); hold = false; }
        }
        else 
        {
            mt.color=new Color(1, .24f, .24f, .5f);
        }
    }

    private void _ExitVR()
    {
        PlatformManager.instance.ExitRoom();
    }
}
