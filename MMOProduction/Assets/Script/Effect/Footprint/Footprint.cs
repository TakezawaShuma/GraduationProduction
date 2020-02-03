//
// Footprint.cs
//
// Author: 
//
// 足跡を表示するシェーダー
//

using System.Collections.Generic;
using UnityEngine;


public class Footprint : MonoBehaviour
{

    [SerializeField]
    private GameObject _footprintObj = null;

    [SerializeField]
    private float _updateDistanceNum = 1f;

    [SerializeField]
    private float _life = 0.5f;

    private List<GameObject> _footprintList;

    private GameObject _footprintParent;

    private Vector3 _lastPos;
    private Vector3 _currentPos;

    private float _lifeCount;


    private void Start()
    {
        _footprintList = new List<GameObject>();

        _footprintParent = new GameObject();
        _footprintParent.name = "Footprints";

        _lastPos = this.transform.position;
        _currentPos = _lastPos;

        _lifeCount = _life;
    }

    private void Update()
    {
        _lifeCount -= Time.deltaTime;
        if (_lifeCount <= 0)
        {
            if (_footprintList.Count != 0)
            {
                GameObject destroyObj = _footprintList[0];
                _footprintList.RemoveAt(0);
                Destroy(destroyObj);
            }

            _lifeCount = _life;
        }

        // 現在地更新
        _currentPos = this.transform.position;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "PlayScene") return;
        if (_currentPos.x >= 0 || _currentPos.z >= 0) return;

        // 一定数進んだ時に足跡を生成
        if (Vector3.Distance(_currentPos, _lastPos) >= _updateDistanceNum)
        {
            GameObject go = Instantiate(_footprintObj);
            go.transform.rotation = Quaternion.Euler(
                0,
                this.transform.rotation.y,
                0
                );
            go.transform.position = new Vector3(
                this.transform.position.x,
                this.transform.position.y,
                this.transform.position.z
                );
            //go.transform.parent = _footprintParent.transform;
            _footprintList.Add(go);

            _lastPos = _currentPos;
        }

        
    }
}