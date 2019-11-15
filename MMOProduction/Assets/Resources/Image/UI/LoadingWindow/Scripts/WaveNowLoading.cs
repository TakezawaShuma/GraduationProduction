//
// WaveNowLoading.cs
// Author: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 「NowLoading」画像を一定時間ごとにうねらせる
/// </summary>
public class WaveNowLoading : MonoBehaviour
{

    [SerializeField] float _waveSpeed = 5;
    [SerializeField] float _waveHeight = 10;

    private List<Text> _imageList;    // ロード中のテキスト
    private float _time;


    private void Start()
    {
        _imageList = new List<Text>();
        _time = 0;

        // 自身の子のテキストをリストに追加
        for (int i = 0; i < transform.childCount; i++)
        {
            Text t = transform.GetChild(i).GetComponent<Text>();
            _imageList.Add(t);
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;

        int count = 0;
        foreach (var text in _imageList)
        {
            Vector3 pos = text.rectTransform.position;
            float y = Mathf.Sin((Time.time - count * 0.1f) * _waveSpeed) * _waveHeight;
            pos.y = y;
            pos.y = Mathf.Max(pos.y, 0);
            text.rectTransform.position = pos;
            count++;
        }
    }
}
