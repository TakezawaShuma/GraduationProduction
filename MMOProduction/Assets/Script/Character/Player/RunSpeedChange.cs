using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpeedChange : MonoBehaviour
{
    [SerializeField, Header("速度上昇コマンド")]
    private Command upCommand = null;

    [SerializeField, Header("速度リセットコマンド")]
    private Command resetCommand = null;

    private float startRunSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
