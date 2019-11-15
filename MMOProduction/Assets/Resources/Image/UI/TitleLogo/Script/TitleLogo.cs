//
// TitleLogo.cs
//
// Author : Tama
//
// タイトルロゴの光沢を表現するために使用
//

using UnityEngine;

public class TitleLogo : MonoBehaviour
{

    [SerializeField]
    private Material _material = null;

    private float _luster;


    private void Start()
    {
        _luster = 0;
    }

    private void Update()
    {
        UpdateLuster();
        _material.SetFloat("_Luster", _luster);
    }

    private void UpdateLuster()
    {
        _luster += 0.1f;
        _luster = (_luster >= 10) ? 0 : _luster;
    }
}
