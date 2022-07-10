using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectClassroom : MonoBehaviour
{
    [SerializeField] GameObject classBtn, SeaclassBtn, SpaceclassBtn;
    void Start()
    {
        if (!Playfabmanager._instance.getTeacher())
        {
            classBtn.SetActive(false);
            SeaclassBtn.SetActive(false);
            SpaceclassBtn.SetActive(false);
            SelectRoom(0);
        }
        else { SelectRoom(1); }
    }
    public void SelectRoom(int roomID)// 1 class : 2 sea : 3 space
    {
        if (roomID == 1)
        {
            classBtn.GetComponent<Image>().color = new Color(0, .5f, 0, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            PlayerPrefs.SetString("classRoom", "Room");
        }
        else if (roomID == 2)
        {
            classBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(0, .5f, 0, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            PlayerPrefs.SetString("classRoom", "Searoom");
        }
        else if (roomID == 2)
        {
            classBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(0, .5f, 0, 1);
            PlayerPrefs.SetString("classRoom", "Spaceroom");
        }
        else if (roomID == 0)
        {
            PlayerPrefs.SetString("classRoom", "Student");
        }


    }


}
