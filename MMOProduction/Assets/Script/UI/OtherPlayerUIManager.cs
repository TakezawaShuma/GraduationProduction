﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerUIManager : MonoBehaviour
{
    public GameObject[] prefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool Observar(int data, int current)
    {
        if (current != data)
        {
            return true;
        }
        return false;
    }
}
