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
        if (PlayerPrefs.HasKey("Model")) {
            //var buffer = Instantiate(boyPrefs[PlayerPrefs.GetInt("BoyModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            var buffer = boyPrefs[PlayerPrefs.GetInt("Model")];
            buffer.SetActive(true);
            buffer.GetComponent<BoyTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                buffer.GetComponent<BoyTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest"), PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet"));
            }
        }
    }

    void LoadGirl() {
        if (PlayerPrefs.HasKey("Model")) {
            //var buffer = Instantiate(girlPrefs[PlayerPrefs.GetInt("GirlModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            var buffer = girlPrefs[PlayerPrefs.GetInt("Model")];
            buffer.SetActive(true);
            buffer.GetComponent<GirlTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                buffer.GetComponent<GirlTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest"), PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet"));
            }
        }
    }
}
