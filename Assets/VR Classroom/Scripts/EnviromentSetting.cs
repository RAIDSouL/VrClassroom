using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentSetting : MonoBehaviour
{
   [SerializeField] GameObject roomWall;
   [SerializeField] GameObject roomRoof;
   [SerializeField] GameObject roomlight;

   [SerializeField] GameObject SeaRoom;

   [SerializeField] GameObject SpaceRoom;

    void Start()
    {
        if (PlayerPrefs.GetString("classRoom") == "Room") { }
        else if (PlayerPrefs.GetString("classRoom") == "Searoom") { }
        else if (PlayerPrefs.GetString("classRoom") == "Spaceroom") { }
        else if (PlayerPrefs.GetString("classRoom") == "Student"){/**/}
    }

}
