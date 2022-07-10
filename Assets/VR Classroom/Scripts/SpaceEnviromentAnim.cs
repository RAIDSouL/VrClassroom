using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnviromentAnim : MonoBehaviour
{
    [SerializeField] GameObject stars;// Start is called before the first frame update
    float rottY;
    private void Start()
    {
        rottY = 0;
    }
    void Update()
    {
        rottY += Time.deltaTime / 10;
        if (rottY > 360) rottY = 0;
        stars.transform.localEulerAngles = new Vector3(0, rottY, 0);
        
    }
}
