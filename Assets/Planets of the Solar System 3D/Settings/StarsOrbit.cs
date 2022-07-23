using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsOrbit : MonoBehaviour
{
    [SerializeField] float OrbitSpeed;
  
    float rotateValue;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotateValue +=  OrbitSpeed * Time.deltaTime;
        gameObject.transform.localEulerAngles = new Vector3(0, rotateValue, 0);
    }
}
