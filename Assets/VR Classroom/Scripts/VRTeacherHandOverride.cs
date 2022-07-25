using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeacherHandOverride : MonoBehaviour
{
   [SerializeField] SkinnedMeshRenderer leftHand, RightHand;
   [SerializeField] Material skinMat;
    public GameObject ObSkinRef, hidehand;
    void Start()
    {

      //  Material skinMatU = new Material(skinMat);
      //  ObSkinRef = gameObject.GetComponentInChildren<JointManager>().transform.GetChild(1).gameObject;
      //  hidehand = gameObject.GetComponentInChildren<JointManager>().transform.GetChild(9).gameObject; 
      //  Material[] skinRef = ObSkinRef.GetComponent<Renderer>().materials;
      //  skinMatU.color = skinRef[0].color;
      //  leftHand.material = skinMatU;
      //  RightHand.material = skinMatU;
      //  hidehand.SetActive(false);
    }

}
