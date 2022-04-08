using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] GameObject[] boyPrefs;
    [SerializeField] GameObject[] girlPrefs;

    private void Start() {
        LoadModel();
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
            //var buffer = Instantiate(boyPrefs[PlayerPrefs.GetInt("BoyModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            var buffer = boyPrefs[PlayerPrefs.GetInt("BoyModel")];
            buffer.SetActive(true);
            buffer.GetComponent<BoyTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("BoySkintone")) {
                buffer.GetComponent<BoyTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("BoySkintone"), PlayerPrefs.GetInt("BoyHair"), PlayerPrefs.GetInt("BoyChest"), PlayerPrefs.GetInt("BoyLeg"), PlayerPrefs.GetInt("BoyFeet"));
            }
        }
    }

    void LoadGirl() {
        if (PlayerPrefs.HasKey("GirlModel")) {
            //var buffer = Instantiate(girlPrefs[PlayerPrefs.GetInt("GirlModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            var buffer = girlPrefs[PlayerPrefs.GetInt("BoyModel")];
            buffer.SetActive(true);
            buffer.GetComponent<GirlTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("GirlSkintone")) {
                buffer.GetComponent<GirlTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("GirlSkintone"), PlayerPrefs.GetInt("GirlHair"), PlayerPrefs.GetInt("GirlChest"), PlayerPrefs.GetInt("GirlLeg"), PlayerPrefs.GetInt("GirlFeet"));
            }
        }
    }
}
