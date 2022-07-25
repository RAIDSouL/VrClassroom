using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsOrbit : MonoBehaviour
{
    [SerializeField] float OrbitSpeed;

    float rotateValue;
    Vector2 startValue;

    void Start()
    {
        startValue.x = gameObject.transform.localEulerAngles.x;
        startValue.y = gameObject.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        rotateValue += OrbitSpeed * Time.deltaTime;
        gameObject.transform.localEulerAngles = new Vector3(startValue.x, startValue.y + rotateValue, 0);
    }
}
