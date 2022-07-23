using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSetting : MonoBehaviour
{
   

    [SerializeField] GameObject schoolRoom;
    [SerializeField] GameObject SeaRoom;

    [SerializeField] GameObject SpaceRoom;

    public void SetClass(int cc)
    {
        if (cc==1) { }
        else if (cc==2)
        {
           
            SeaRoom.SetActive(true);
        }
        else if (cc==3)
        {
           
            SpaceRoom.SetActive(true);
        }
        else if (cc==0) { schoolRoom.SetActive(true); }
    }

}
