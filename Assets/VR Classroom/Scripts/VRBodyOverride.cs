using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiliGames.VRClassroom;
using Photon.Pun;

public class VRBodyOverride : MonoBehaviour
{
    [SerializeField] StudentBodyFollow VR_body;
    [SerializeField] JointManager j_manager;
    [SerializeField] PhotonView PV;
    bool handUp = false;
    Transform r_hand, l_hand, head;
    void Start()
    {
        handUp = false;
        Invoke("settJoint", .5f);
        PV = gameObject.GetComponent<PhotonView>();
        print("pv ++ " + PV.IsMine);
    }

    void settJoint() { 
        j_manager = GetComponentInChildren<JointManager>();
        r_hand = VR_body.body[2];
        l_hand = VR_body.body[1];
        head = VR_body.body[0];
    }
    
    void Update()
    {
        if (j_manager == null) return;
        if ((r_hand.position.y > head.position.y && handUp) || (r_hand.position.y < head.position.y && !handUp)) return;
        if (r_hand.position.y > head.position.y && !handUp) 
        {
            handUp = true;
            j_manager.OnHandup(handUp);
        }
        else if (r_hand.position.y < head.position.y && handUp)
        {
            handUp = false;
            j_manager.OnHandup(handUp);
        }



    }
}
