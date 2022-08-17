using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testevent : MonoBehaviour
{

    void Awake()
    {
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    { 
    }

    private void Start()
    {
    
       
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
    }
}
