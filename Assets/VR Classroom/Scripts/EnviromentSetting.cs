using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSetting : MonoBehaviour
{
    [SerializeField] Camera camT, camS;

    [SerializeField] GameObject schoolRoom;
    [SerializeField] GameObject SeaRoom;
    [SerializeField] GameObject SpaceRoom;

    [SerializeField] Material classSky;
    [SerializeField] Material spaceSky;

    public void SetClass(int cc)
    {
        if (cc == 0 || cc == 1)//classroom
        {
            RenderSettings.skybox = classSky;
            camT.clearFlags = CameraClearFlags.Skybox;
            camS.clearFlags = CameraClearFlags.Skybox;
            schoolRoom.SetActive(true);

        }
        else if (cc == 2)//SEA
        {
            camT.clearFlags = CameraClearFlags.SolidColor;
            camS.clearFlags = CameraClearFlags.SolidColor;
            SeaRoom.SetActive(true);
        }
        else if (cc == 3)//SPACE
        {
            RenderSettings.skybox = spaceSky;
            camT.clearFlags = CameraClearFlags.Skybox;
            camS.clearFlags = CameraClearFlags.Skybox;
            SpaceRoom.SetActive(true);
        }

    }

}
