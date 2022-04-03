using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWNER : MonoBehaviour {

    public GameObject[] characters;    
    float randomTime;
    float timeCounter;
    public float deviation;
    int coin;

    
	
	void Update ()
    {
        if (timeCounter > randomTime)
        {
            coin = Random.Range(0, 6);
            GameObject newcharacter = Instantiate(characters[coin], transform.position + (transform.right * Random.Range(-1f, 1f)),transform.rotation * Quaternion.Euler(Vector3.up * Random.Range(-deviation, deviation)));
            if (coin < 3)
            {                
                newcharacter.GetComponent<GirlTKPrefabMaker>().Getready();
                newcharacter.GetComponent<GirlTKPrefabMaker>().Randomize();
                newcharacter.GetComponent<playanimation>().playtheanimation("TK_walk1");
                newcharacter.GetComponent<GirlTKPrefabMaker>().FIX();
            }
            else
            {             
                newcharacter.GetComponent<BoyTKPrefabMaker>().Getready();
                newcharacter.GetComponent<BoyTKPrefabMaker>().Randomize();
                newcharacter.GetComponent<playanimation>().playtheanimation("TK_walk1");
                newcharacter.GetComponent<BoyTKPrefabMaker>().FIX();
            }

            randomTime = Random.Range(1, 4);
            timeCounter = 0f;
        }
        timeCounter += Time.deltaTime;
    }
}
