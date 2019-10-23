//
// RotateSkybox.cs
// 
// Author : Tamamura Shuuki
//

using UnityEngine;


// 簡易ローテーションクラス
// Skybox用
public class RotateSkybox : MonoBehaviour
{

    [SerializeField]
    private Material _skybox = null;

    public float _speed;

    private float _rotation;


    private void Start()
    {
        _rotation = 0;
    }

    private void Update()
    {
        _rotation += _speed * Time.deltaTime;
        _skybox.SetFloat("_Rotation", _rotation);
    }
}
