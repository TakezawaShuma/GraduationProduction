using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップのコントローラークラス
/// </summary>
[RequireComponent(typeof(Camera))]
public class MiniMapController : MonoBehaviour
{
    // プレイヤー
    private GameObject target_ = null;
    private float height_ = 20;

    [SerializeField]
    private Shader _shader = null;

    [SerializeField]
    private RenderTexture _targetTexure = null;
    [SerializeField]
    private RenderTexture _minimapTexure = null;

    // シェーダー用プロパティ構造体
    [System.Serializable]
    public struct ShaderProperty
    {
        public Color raderColor;
        public float raderSize;
    }
    [SerializeField]
    private ShaderProperty _shaderProperty;

    private Material _material;


    public void Init(GameObject _target) {
        target_ = _target;

        _material = new Material(_shader);
        _material.SetTexture("_TargetTex", _targetTexure);
        _material.SetColor("_RaderColor", _shaderProperty.raderColor);
        _material.SetFloat("_RaderSize", _shaderProperty.raderSize);
    }
    // Update is called once per frame
    void Update() {
        if (target_ == null) return;
        transform.position = new Vector3(target_.transform.position.x, height_, target_.transform.position.z);
        transform.eulerAngles = new Vector3(90, Camera.main.transform.eulerAngles.y, 0);

        Graphics.SetRenderTarget(null);
        Graphics.Blit(_targetTexure, _minimapTexure, _material);
    }
}
