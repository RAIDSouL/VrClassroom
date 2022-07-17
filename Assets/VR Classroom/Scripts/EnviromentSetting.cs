using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSetting : MonoBehaviour
{
    [SerializeField] GameObject roomWall;
    [SerializeField] GameObject roomRoof;
    [SerializeField] GameObject roomfloor;

    [SerializeField] GameObject SeaRoom;

    [SerializeField] GameObject SpaceRoom;

    public void SetClass(int cc)
    {
        if (cc==1) { }
        else if (cc==2)
        {
            roomWall.SetActive(false);
            roomRoof.SetActive(false);
           // roomfloor.SetActive(false);
            SeaRoom.SetActive(true);
        }
        else if (cc==3)
        {
            roomWall.SetActive(false);
            roomRoof.SetActive(false);
           // roomfloor.SetActive(false);
            SpaceRoom.SetActive(true);
        }
        else if (cc==0) {/**/}
    }

}
