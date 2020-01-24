using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミニマップのコントローラークラス
/// </summary>
public class MiniMapController : MonoBehaviour
{
    // プレイヤー
    private GameObject target_ = null;
    private float height_ = 20;

    [SerializeField]
    private RenderTexture _colorTexture = null;
    [SerializeField]
    private RenderTexture _minimapTexture = null;
    [SerializeField]
    private Material _material = null;


    public void Init(GameObject _target) {
        target_ = _target;
    }
    // Update is called once per frame
    void Update() {
        if (target_ == null) return;
        transform.position = new Vector3(target_.transform.position.x, height_, target_.transform.position.z);
        transform.eulerAngles = new Vector3(90, Camera.main.transform.eulerAngles.y, 0);
    }

    private void OnPostRender()
    {
        Graphics.SetRenderTarget(_colorTexture);
        Graphics.Blit(_colorTexture, _minimapTexture, _material);
    }
}
