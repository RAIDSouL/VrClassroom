using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSetting : MonoBehaviour
{
    [SerializeField] Camera camT, camS;

    [SerializeField] GameObject schoolRoom;
    [SerializeField] GameObject SeaRoom;

    [SerializeField] GameObject SpaceRoom;

    public void SetClass(int cc)
    {
        if (cc == 0 || cc == 1)
        {
            camT.clearFlags = CameraClearFlags.Skybox;
            camS.clearFlags = CameraClearFlags.Skybox;
            schoolRoom.SetActive(true);

        }
        else if (cc == 2)
        {

            SeaRoom.SetActive(true);
        }
        else if (cc == 3)
        {
            SpaceRoom.SetActive(true);
        }

    }

}
