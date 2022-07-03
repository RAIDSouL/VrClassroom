using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharecterEditor : MonoBehaviour {
    public static CharecterEditor _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        }
    }

    [Header("Gender Setup")]
    [SerializeField] GameObject sexPanel;
    [SerializeField] GameObject[] boyPrefs;
    [SerializeField] GameObject[] girlPrefs;
    [SerializeField] Transform spawnPos;
    [SerializeField] GameObject confirmPanel;
    [SerializeField] Button oldSaveBtn;

    [Header("Customize")]
    [SerializeField] GameObject customizePanel;

    //cache
    int boyPrefIndex = 0;
    int girlPrefIndex = 0;
    bool isMaleGender = false;
    GirlVRTKPrefabMaker? girlTemp = null;
    BoyVRTKPrefabMaker? boyTemp = null;
    [SerializeField] GameObject? avatar = null;

    //Access
    public bool IsMaleGender { get { return isMaleGender; } set { isMaleGender = value; } }
    public int BoyPrefIndex { get { return boyPrefIndex; } set { boyPrefIndex = value; } }
    public int GirlPrefIndex { get { return girlPrefIndex; } set { girlPrefIndex = value; } }
    public GameObject ChildObj { get { return childObj; } set { childObj = value; } }

    GameObject childObj;

    private void Start() {
        //PlayerPrefs.DeleteAll();
        //LoadModel();
        childObj = transform.GetChild(0).gameObject;
    }
    
    public void TogglePanel(bool isOn) {
        childObj.SetActive(isOn);//base  
        if (Playfabmanager._instance.hasCharacterSave)
        {
            if (PlayerPrefs.GetInt("Gender") == 0)
            {
                isMaleGender = true; if(isOn)LoadBoy();
            }
            else
            {
                isMaleGender = false; if(isOn) LoadGirl();
            }

            LobbyCanvas.instance.JoinGroup.SetActive(true);
            sexPanel.transform.localScale = new Vector3(1,0,1);
        }
    }

    public void ChooseBoy(bool index) {
        isMaleGender = index;
        ReloadAvatar();
        HideGenderPanel();
    }

    private void HideGenderPanel() {
        sexPanel.transform.DOScaleY(0f, .25f).SetEase(Ease.InBack);
        confirmPanel.SetActive(true);
        confirmPanel.transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutBack).SetDelay(.5f);
    }

    public void ReloadAvatar() {
        if (avatar != null) {
            Destroy(avatar);
            avatar = null;
        }
        if (isMaleGender) {
            boyPrefIndex = boyPrefIndex + 1 < boyPrefs.Length ? boyPrefIndex + 1 : 0;
            avatar = Instantiate(boyPrefs[boyPrefIndex], spawnPos.position, spawnPos.rotation) as GameObject;
            boyTemp = avatar.GetComponent<BoyVRTKPrefabMaker>();
            boyTemp.Getready();
            if (boyTemp.hatactive) {
                boyTemp.HatOn();
            }
            if (boyTemp.glassesactive) {
                boyTemp.Glasseson();
            }
        } else {
            girlPrefIndex = girlPrefIndex + 1 < girlPrefs.Length ? girlPrefIndex + 1 : 0;
            avatar = Instantiate(girlPrefs[girlPrefIndex], spawnPos.position, spawnPos.rotation) as GameObject;
            girlTemp = avatar.GetComponent<GirlVRTKPrefabMaker>();
            girlTemp.Getready();
            if (girlTemp.hatactive) {
                girlTemp.Prevhair();
            }
            if (girlTemp.glassesactive) {
                girlTemp.Glasseson();
            }
        }
        Debug.Log("Boy: " + boyTemp + " Girl: " + girlTemp);
    }

    public void SetAvatar() {
        confirmPanel.transform.DOScaleY(0f, .5f).SetEase(Ease.InBack);
        customizePanel.transform.DOScaleY(1f, .5f).SetEase(Ease.OutBack).SetDelay(.5f);
    }

    public void CallPrevHair() {
        if (isMaleGender) {
            boyTemp.Prevhair();
        } else {
            girlTemp.Prevhair();
        }
    }

    public void CallNextHair() {
        if (isMaleGender) {
            boyTemp.Nexthair();
        } else {
            girlTemp.Nexthair();
        }
    }

    public void CallPrevChest() {
        if (isMaleGender) {
            boyTemp.Prevchest();
        } else {
            girlTemp.Prevchest();
        }
    }

    public void CallNextChest() {
        if (isMaleGender) {
            boyTemp.Nextchest();
        } else {
            girlTemp.Nextchest();
        }
    }

    //public void CallPrevLeg() {
    //    if (isMaleGender) {
    //        boyTemp.Prevlegs();
    //    } else {
    //        girlTemp.Prevlegs();
    //    }
    //}

    //public void CallNextLeg() {
    //    if (isMaleGender) {
    //        boyTemp.Nextlegs();
    //    } else {
    //        girlTemp.Nextlegs();
    //    }
    //}

    //public void CallPrevFeet() {
    //    if (isMaleGender) {
    //        boyTemp.Prevfeet();
    //    } else {
    //        girlTemp.Prevfeet();
    //    }
    //}

    //public void CallNextFeet() {
    //    if (isMaleGender) {
    //        boyTemp.Nextfeet();
    //    } else {
    //        girlTemp.Nextfeet();
    //    }
    //}

    public void CallNextSkinColor(int index) {
        if (isMaleGender) {
            boyTemp.Nextskincolor(index);
        } else {
            girlTemp.Nextskincolor(index);
        }
    }

    public void SaveAvatar() {
        if (IsMaleGender) {
            PlayerPrefs.SetInt("Gender", 0);
        } else {
            PlayerPrefs.SetInt("Gender", 1);
        }

        if (isMaleGender) {
            boyTemp.FIX();
            SaveModelBoy(boyTemp);
        } else {
            girlTemp.FIX();
            SaveModelGirl(girlTemp);
        }

     //   FindObjectOfType<Animator>().gameObject.SetActive(false);
        TogglePanel(false);
        LobbyCanvas.instance.JoinGroup.SetActive(true);
    }

    void SaveModelBoy(BoyVRTKPrefabMaker boy) {
        Debug.Log("Hair : " + boy.Hair + " Skintone : " + boy.Skintone + " Chest : " + boy.Chest /*+ " Leg : " + boy.Legs + " Feet : " + boy.Feet*/);
        PlayerPrefs.SetInt("Gender", 0);
        PlayerPrefs.SetInt("Model", BoyPrefIndex);
        PlayerPrefs.SetInt("Hair", boy.Hair);
        PlayerPrefs.SetInt("Skintone", boy.Skintone);
        PlayerPrefs.SetInt("Chest", boy.Chest);
        //PlayerPrefs.SetInt("Leg", boy.Legs);
        //PlayerPrefs.SetInt("Feet", boy.Feet);
        Playfabmanager._instance.PlayFabSaveAvatar(BoyPrefIndex, boy);
    }

    void SaveModelGirl(GirlVRTKPrefabMaker girl) {
        Debug.Log("Hair : " + girl.Hair + " Skintone : " + girl.Skintone + " Chest : " + girl.Chest/* + " Leg : " + girl.Legs + " Feet : " + girl.Feet*/);
        PlayerPrefs.SetInt("Gender", 1);
        PlayerPrefs.SetInt("Model", GirlPrefIndex);
        PlayerPrefs.SetInt("Hair", girl.Hair);
        PlayerPrefs.SetInt("Skintone", girl.Skintone);
        PlayerPrefs.SetInt("Chest", girl.Chest);
        //PlayerPrefs.SetInt("Leg", girl.Legs);
        //PlayerPrefs.SetInt("Feet", girl.Feet);
        Playfabmanager._instance.PlayFabSaveAvatar(GirlPrefIndex, girl);
    }
    
    public void LoadModel() {
        if (PlayerPrefs.HasKey("Gender")) {
            if (PlayerPrefs.GetInt("Gender") == 0) {
                LoadBoy();
            } else if (PlayerPrefs.GetInt("Gender") == 1) {
                LoadGirl();
            }
        }
        HideGenderPanel();
    }

    void LoadBoy() {
        if (PlayerPrefs.HasKey("Model")) {
            avatar = Instantiate(boyPrefs[PlayerPrefs.GetInt("Model")], spawnPos.position, spawnPos.rotation) as GameObject;
            boyTemp = avatar.GetComponent<BoyVRTKPrefabMaker>();
            boyTemp.Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                boyTemp.LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest")/*, PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet")*/);
            }
            if (boyTemp.hatactive) {
                boyTemp.HatOn();
            }
            if (boyTemp.glassesactive) {
                boyTemp.Glasseson();
            }
            Debug.Log("Hair : " + boyTemp.Hair + " Skintone : " + boyTemp.Skintone + " Chest : " + boyTemp.Chest /*+ " Leg : " + boy.Legs + " Feet : " + boy.Feet*/);
        }
       
    }

    void LoadGirl() {
        if (PlayerPrefs.HasKey("Model")) {
            avatar = Instantiate(girlPrefs[PlayerPrefs.GetInt("Model")], spawnPos.position, spawnPos.rotation) as GameObject;
            girlTemp = avatar.GetComponent<GirlVRTKPrefabMaker>();
            girlTemp.Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                girlTemp.LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest")/*, PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet")*/);
            }
            if (girlTemp.hatactive) {
                girlTemp.HatOn();
            }
            if (girlTemp.glassesactive) {
                girlTemp.Glasseson();
            }
        }
    }
    public void Re_EditChar() {
        if (!childObj.activeSelf) { childObj.SetActive(true); }
        PlayerPrefs.SetInt("Gender", 0);
        PlayerPrefs.SetInt("Model", 0);
        PlayerPrefs.SetInt("Hair", 0);
        PlayerPrefs.SetInt("Skintone", 0);
        PlayerPrefs.SetInt("Chest", 0);
        LobbyCanvas.instance.JoinGroup.SetActive(false);
        sexPanel.transform.DOScaleY(1f, 0.5f).SetEase(Ease.OutBack).SetDelay(.5f);
    }
    public void CallOldSaveModel() {
        
        oldSaveBtn.interactable = true;
    }
}
