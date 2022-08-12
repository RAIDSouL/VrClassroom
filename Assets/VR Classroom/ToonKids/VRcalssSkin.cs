using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRcalssSkin : MonoBehaviour
{
    [SerializeField] Material[] skin;
    [SerializeField] SkinnedMeshRenderer Head, hand;
    [SerializeField] SkinnedMeshRenderer[] Body;
    public void setSkin(int a)
    {
        if (a > 0)
            a--;

        Material[] tempMat = Head.materials;
        tempMat[0] = skin[a];
        Head.materials = tempMat;

        tempMat = hand.materials;
        tempMat[0] = skin[a];
        hand.materials = tempMat;

        foreach (SkinnedMeshRenderer mr in Body)
        {
            if (mr.gameObject.activeSelf)
            {
                tempMat = mr.materials;
                tempMat[1] = skin[a];
                mr.materials = tempMat;

            }
        }

    }

}
