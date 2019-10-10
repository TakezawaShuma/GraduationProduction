﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugIP : MonoBehaviour
{

    Text ipText;
    private void Awake()
    {
        ipText = transform.GetComponent<Text>();
        Retention.IP = ipText.text;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Retention.IP = ipText.text;
    }

}
