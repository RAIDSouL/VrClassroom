using DG.Tweening;
using UnityEngine;

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

    [Header("Customize")]
    [SerializeField] GameObject customizePanel;

    //cache
    int boyPrefIndex = 0;
    int girlPrefIndex = 0;
    bool isMaleGender = false;
    GirlTKPrefabMaker? girlTemp = null;
    BoyTKPrefabMaker? boyTemp = null;
    GameObject? avatar = null;

    //Access
    public bool IsMaleGender { get { return isMaleGender; } set { isMaleGender = value; } }
    public int BoyPrefIndex { get { return boyPrefIndex; } set { boyPrefIndex = value; } }
    public int GirlPrefIndex { get { return girlPrefIndex; } set { girlPrefIndex = value; } }

    private void Start() {
        //PlayerPrefs.DeleteAll();
        LoadModel();
    }

    public void TogglePanel(bool index) {
        transform.GetChild(0).gameObject.SetActive(index);
    }

    public void ChooseBoy(bool index) {
        isMaleGender = index;
        ReloadAvatar();
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
            boyTemp = avatar.GetComponent<BoyTKPrefabMaker>();
            boyTemp.Getready();
            if (boyTemp.hatactive) {
                boyTemp.HatOn();
            }
        } else {
            girlPrefIndex = girlPrefIndex + 1 < girlPrefs.Length ? girlPrefIndex + 1 : 0;
            avatar = Instantiate(girlPrefs[girlPrefIndex], spawnPos.position, spawnPos.rotation) as GameObject;
            girlTemp = avatar.GetComponent<GirlTKPrefabMaker>();
            girlTemp.Getready();
            if (girlTemp.hatactive) {
                girlTemp.Prevhair();
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

    public void CallPrevLeg() {
        if (isMaleGender) {
            boyTemp.Prevlegs();
        } else {
            girlTemp.Prevlegs();
        }
    }

    public void CallNextLeg() {
        if (isMaleGender) {
            boyTemp.Nextlegs();
        } else {
            girlTemp.Nextlegs();
        }
    }

    public void CallPrevFeet() {
        if (isMaleGender) {
            boyTemp.Prevfeet();
        } else {
            girlTemp.Prevfeet();
        }
    }

    public void CallNextFeet() {
        if (isMaleGender) {
            boyTemp.Nextfeet();
        } else {
            girlTemp.Nextfeet();
        }
    }

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

        TogglePanel(false);
        LobbyCanvas.instance.JoinGroup.SetActive(true);
    }

    void SaveModelBoy(BoyTKPrefabMaker boy) {
        Debug.Log("Hair : " + boy.Hair + " Skintone : " + boy.Skintone + " Chest : " + boy.Chest + " Leg : " + boy.Legs + " Feet : " + boy.Feet);
        PlayerPrefs.SetInt("BoyModel", BoyPrefIndex);
        PlayerPrefs.SetInt("BoyHair", boy.Hair);
        PlayerPrefs.SetInt("BoySkintone", boy.Skintone);
        PlayerPrefs.SetInt("BoyChest", boy.Chest);
        PlayerPrefs.SetInt("BoyLeg", boy.Legs);
        PlayerPrefs.SetInt("BoyFeet", boy.Feet);
    }

    void SaveModelGirl(GirlTKPrefabMaker girl) {
        Debug.Log("Hair : " + girl.Hair + " Skintone : " + girl.Skintone + " Chest : " + girl.Chest + " Leg : " + girl.Legs + " Feet : " + girl.Feet);
        PlayerPrefs.SetInt("GirlModel", GirlPrefIndex);
        PlayerPrefs.SetInt("GirlHair", girl.Hair);
        PlayerPrefs.SetInt("GirlSkintone", girl.Skintone);
        PlayerPrefs.SetInt("GirlChest", girl.Chest);
        PlayerPrefs.SetInt("GirlLeg", girl.Legs);
        PlayerPrefs.SetInt("GirlFeet", girl.Feet);
    }

    void LoadModel() {
        if (PlayerPrefs.HasKey("Gender")) {
            if (PlayerPrefs.GetInt("Gender") == 0) {
                LoadBoy();
            } else if (PlayerPrefs.GetInt("Gender") == 1) {
                LoadGirl();
            }
        }
    }

    void LoadBoy() {
        if (PlayerPrefs.HasKey("BoyModel")) {
            var buffer = Instantiate(boyPrefs[PlayerPrefs.GetInt("BoyModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            buffer.GetComponent<BoyTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("BoySkintone")) {
                buffer.GetComponent<BoyTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("BoySkintone"), PlayerPrefs.GetInt("BoyHair"), PlayerPrefs.GetInt("BoyChest"), PlayerPrefs.GetInt("BoyLeg"), PlayerPrefs.GetInt("BoyFeet"));
            }
        }
    }

    void LoadGirl() {
        if (PlayerPrefs.HasKey("GirlModel")) {
            var buffer = Instantiate(girlPrefs[PlayerPrefs.GetInt("GirlModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            buffer.GetComponent<GirlTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("GirlSkintone")) {
                buffer.GetComponent<GirlTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("GirlSkintone"), PlayerPrefs.GetInt("GirlHair"), PlayerPrefs.GetInt("GirlChest"), PlayerPrefs.GetInt("GirlLeg"), PlayerPrefs.GetInt("GirlFeet"));
            }
        }
    }
}
