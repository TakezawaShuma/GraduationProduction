//
// CameraWork.cs
//
// Author: Tama
//
// カメラワークを管理するクラス
//

using System.Collections.Generic;
using UnityEngine;


public class CameraWork : MonoBehaviour
{

    /// <summary>
    /// カメラのワークモードID
    /// </summary>
    public enum WorkMode
    {
        One = 0b00000001,
        Loop = 0b00000010,
    }


    // カメラを登録
    [SerializeField]
    private GameObject[] _cameras = new GameObject[1];

    // 使用するカメラリスト
    private List<WorkingCamera> _workingCameraList;

    // 活動中カメラのインデックス番号
    private int _currentWorkIndex;


    private void Start()
    {
        // カメラをリストに登録
        _workingCameraList = new List<WorkingCamera>();
        foreach (var camera in _cameras)
        {
            camera.SetActive(false);
            _workingCameraList.Add(camera.GetComponent<WorkingCamera>());
        }
        _workingCameraList[0].gameObject.SetActive(true);

        _currentWorkIndex = 0;
    }

    private void Update()
    {
        if (_currentWorkIndex < 0) return;

        WorkingCamera.WorkState state = _workingCameraList[_currentWorkIndex].OnUpdate();
        if (state == WorkingCamera.WorkState.Finised)
        {
            _workingCameraList[_currentWorkIndex].Exit();

            _currentWorkIndex++;
            if (_currentWorkIndex >= _workingCameraList.Count)
            {
                _currentWorkIndex = 0;
            }

            _workingCameraList[_currentWorkIndex].Play();
        }
    }

    public void AddCamera(WorkingCamera camera)
    {
        _workingCameraList.Add(camera);
    }
}
