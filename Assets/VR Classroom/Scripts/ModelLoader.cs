using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] GameObject[] boyPrefs;
    [SerializeField] GameObject[] girlPrefs;

    private void Start() {
        //LoadModel();
    }

    public GameObject Load(int Gender, int Model, int Hair, int Skintone, int Chest/*, int Leg, int Feet*/)
    {
        GameObject character = null;
     if (Gender == 0)
     {
         character = LoadBoy(Model, Hair, Skintone, Chest/*, Leg, Feet*/);
     }
     else if (Gender == 1)
     {
         character = LoadGirl(Model, Hair, Skintone, Chest/*, Leg, Feet*/);
     }
        return character;
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
            buffer.GetComponent<BoyVRTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                buffer.GetComponent<BoyVRTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest")/*, PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet")*/);
            }
        }
    }
    GameObject LoadBoy(int Model, int Hair, int Skintone, int Chest/*, int Leg, int Feet*/)
    {
        GameObject buffer = boyPrefs[Model];
        buffer.SetActive(true);
        BoyVRTKPrefabMaker boyTemp = buffer.GetComponent<BoyVRTKPrefabMaker>();
        boyTemp.Getready();
        boyTemp.LoadOldModel(Skintone, Hair, Chest/*, Leg, Feet*/);
        if (boyTemp.hatactive) {
            boyTemp.HatOn();
        }
        if (boyTemp.glassesactive) {
            boyTemp.Glasseson();
        }
        GameObject character = Instantiate(buffer);
        buffer.SetActive(false);
        return character;
    }

    void LoadGirl() {
        if (PlayerPrefs.HasKey("Model")) {
            //var buffer = Instantiate(girlPrefs[PlayerPrefs.GetInt("GirlModel")], spawnPos.position, spawnPos.rotation) as GameObject;
            var buffer = girlPrefs[PlayerPrefs.GetInt("Model")];
            buffer.SetActive(true);
            buffer.GetComponent<GirlVRTKPrefabMaker>().Getready();
            if (PlayerPrefs.HasKey("Skintone")) {
                buffer.GetComponent<GirlVRTKPrefabMaker>().LoadOldModel(PlayerPrefs.GetInt("Skintone"), PlayerPrefs.GetInt("Hair"), PlayerPrefs.GetInt("Chest")/*, PlayerPrefs.GetInt("Leg"), PlayerPrefs.GetInt("Feet")*/);
            }
        }
    }

    GameObject LoadGirl(int Model, int Hair, int Skintone, int Chest/*, int Leg, int Feet*/)
    {
        //var buffer = Instantiate(girlPrefs[PlayerPrefs.GetInt("GirlModel")], spawnPos.position, spawnPos.rotation) as GameObject;
        GameObject buffer = girlPrefs[Model];
        buffer.SetActive(true);
        GirlVRTKPrefabMaker girlTemp = buffer.GetComponent<GirlVRTKPrefabMaker>();
        girlTemp.Getready();
        girlTemp.LoadOldModel(Skintone, Hair, Chest/*, Leg, Feet*/);
        if (girlTemp.hatactive) {
            girlTemp.HatOn();
        }
        if (girlTemp.glassesactive) {
            girlTemp.Glasseson();
        }
        GameObject character = Instantiate(buffer);
        buffer.SetActive(false);
        return character;
    }
}
