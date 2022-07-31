using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRclassCharacterCreater : MonoBehaviour
{
    [SerializeField] GameObject boy, girl;
    public int Gender;//0 boy : 1 girl
    public SkinnedMeshRenderer HairR, HeadR, HandsR, BodyR;

    public SkinnedMeshRenderer MHairR, MHeadR, MBodyR, MHandsR;
    public SkinnedMeshRenderer FHairR, FHeadR, FBodyR, FHandsR;

    //preset
    [SerializeField] Mesh[] MHairM, FHairM;
    [SerializeField] Material[] HairT, BodyT, SkinT;
    [SerializeField] Material[] FHairT, FBodyT, FSkinT;

    //Value
    [SerializeField] int hairID, skinID, bodyID;

    public int GetCharacter;
    public void CreateCharacter()
    {
        if (Gender == 0)
        {
            HairR.sharedMesh = MHairM[hairID];
            HairR.material = HairT[hairID];

            Material[] tempSkin = BodyR.materials;
            tempSkin[1] = SkinT[skinID];
            BodyR.materials = tempSkin;

            tempSkin = HandsR.materials;
            tempSkin[0] = SkinT[skinID];
            HandsR.materials = tempSkin;

            tempSkin = HeadR.materials;
            tempSkin[0] = SkinT[skinID];
            HeadR.materials = tempSkin;

            tempSkin = BodyR.materials;
            tempSkin[0] = BodyT[bodyID];
            BodyR.materials = tempSkin;
        }
        else
        {
            HairR.sharedMesh = FHairM[hairID];
            HairR.material = FHairT[hairID];

            Material[] tempSkin = BodyR.materials;
            tempSkin[1] = FSkinT[skinID];
            BodyR.materials = tempSkin;

            tempSkin = HandsR.materials;
            tempSkin[0] = FSkinT[skinID];
            HandsR.materials = tempSkin;

            tempSkin = HeadR.materials;
            tempSkin[0] = FSkinT[skinID];
            HeadR.materials = tempSkin;

            tempSkin = BodyR.materials;
            tempSkin[0] = FBodyT[bodyID];
            BodyR.materials = tempSkin;

        }


    }
    public void _NxtHair(bool add)
    {
        if (add)
        {
            if (hairID == MHairM.Length - 1) hairID = 0;
            else hairID++;
        }
        else
        {
            if (hairID == 0) hairID = MHairM.Length - 1;
            else hairID--;
        }
        CreateCharacter();
    }
    public void _NxtSkin(bool add)
    {
        if (add)
        {
            if (skinID == SkinT.Length - 1) skinID = 0;
            else skinID++;
        }
        else
        {
            if (skinID == 0) skinID = SkinT.Length - 1;
            else skinID--;
        }
        CreateCharacter();
    }
    public void _NxtBody(bool add)
    {
        if (add)
        {
            if (bodyID == BodyT.Length - 1) bodyID = 0;
            else bodyID++;
        }
        else
        {
            if (bodyID == 0) bodyID = BodyT.Length - 1;
            else bodyID--;
        }
        CreateCharacter();
    }
    public void _NxtGender()
    {
        if (Gender == 1)
        {
            Gender = 0;
            HairR = MHairR;
            HeadR = MHeadR;
            BodyR = MBodyR;
            HandsR = MHandsR;
            boy.SetActive(true);
            girl.SetActive(false);
        }
        else
        {
            Gender = 1;
            HairR = FHairR;
            HeadR = FHeadR;
            BodyR = FBodyR;
            HandsR = FHandsR;
            boy.SetActive(false);
            girl.SetActive(true);
        }
        hairID = 0; skinID = 0; bodyID = 0;
        CreateCharacter();
    }
    private void Start()
    {
        _NxtGender();
    }
}
