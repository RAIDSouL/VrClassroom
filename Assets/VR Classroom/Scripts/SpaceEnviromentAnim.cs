using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnviromentAnim : MonoBehaviour
{
    [SerializeField] GameObject stars;
    [SerializeField] GameObject starsBG;
    float rottY;
    float rottY2;
    private void Start()
    {
        rottY = 0;
        rottY2 = 0;
    }
    void Update()
    {
        rottY += Time.deltaTime/10;
        rottY2 += Time.deltaTime/3;
        if (rottY > 360) rottY = 0;
        if (rottY2 > 360) rottY = 0;
       // stars.transform.localEulerAngles = new Vector3(0, rottY, 0);
        starsBG.transform.localEulerAngles = new Vector3(0, rottY, 0);
        
    }
}
