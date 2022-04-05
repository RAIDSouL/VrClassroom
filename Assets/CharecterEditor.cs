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
        if (isMaleGender) {
            boyTemp.FIX();
            Debug.Log("Hair : " + boyTemp.Hair + " Skintone : " + boyTemp.Skintone + " Chest : " + boyTemp.Chest + " Leg : " + boyTemp.Legs + " Feet : " + boyTemp.Feet);
        } else {
            girlTemp.FIX();
            Debug.Log("Hair : " + girlTemp.Hair + " Skintone : " + girlTemp.Skintone + " Chest : " + girlTemp.Chest + " Leg : " + girlTemp.Legs + " Feet : " + girlTemp.Feet);
        }

        TogglePanel(false);
        LobbyCanvas.instance.JoinGroup.SetActive(true);
    }
}
