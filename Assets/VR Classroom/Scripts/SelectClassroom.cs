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
            classBtn.GetComponent<Image>().color = new Color(0, .9f, 0, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            PlayerPrefs.SetInt("classRoom", 1);
        }
        else if (roomID == 2)
        {
            classBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(0, .9f, 0, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            PlayerPrefs.SetInt("classRoom", 2);
        }
        else if (roomID == 3)
        {
            classBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            SeaclassBtn.GetComponent<Image>().color = new Color(.85f, .63f, .30f, 1);
            SpaceclassBtn.GetComponent<Image>().color = new Color(0, .9f, 0, 1);
            PlayerPrefs.SetInt("classRoom", 3);
        }
        else if (roomID == 0)
        {
            PlayerPrefs.SetInt("classRoom", 0);
        }


    }


}
