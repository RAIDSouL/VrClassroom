using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeacherHandOverride : MonoBehaviour
{
   [SerializeField] SkinnedMeshRenderer leftHand, RightHand;
   [SerializeField] Material skinMat;
    public GameObject skinRef;
    void Start()
    {

        Material skinMatU = new Material(skinMat);
     
        skinMatU.color=new Color(1,1,1,1);
      
        leftHand.material = skinMatU;
        RightHand.material = skinMatU;
       
    }

  
    void Update()
    {
        
    }
}
