﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private GameObject otherCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.SetActive(!mainCamera.activeSelf);
            otherCamera.SetActive(!otherCamera.activeSelf);
        }
    }
}