//
// WorkingCamera.cs
//
// Author: Tama
//
// カメラワークシステムで使用するカメラ
//

using System;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class WorkingCamera : MonoBehaviour
{

    /// <summary>
    /// カメラの活動状態
    /// </summary>
    public enum WorkState
    { 
        Active,     // 活動
        Inactive,   // 非活動
        Finised,    // 終了
    }


    public Action OnStart { get; set; }
    public Func<WorkState> OnUpdate { get; set; }

    private WorkState _currentWorlkState;


    private void Update()
    {
    }

    public void SetWorkState(WorkState nextState)
    {
        _currentWorlkState = nextState;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        OnStart();
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
