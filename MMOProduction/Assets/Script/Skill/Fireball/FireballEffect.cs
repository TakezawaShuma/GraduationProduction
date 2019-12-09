//
// FireBallEffect.cs
//
// 炎をエフェクトを表現する
//

using UnityEngine;
using System.Runtime.InteropServices;

public struct Fireball
{
    public Vector3 pos;
    public Vector3 startPos;
    public Vector3 accel;
    public Color color;

    public Fireball(Vector3 pos, Vector3 startPos, Vector3 accel, Color color)
    {
        this.pos = pos;
        this.startPos = startPos;
        this.accel = accel;
        this.color = color;
    }
}

public class FireballEffect : MonoBehaviour
{

    [SerializeField] Shader _renderingShader = null;       // 描画用シェーダ
    [SerializeField] Texture _texture = null;
    [SerializeField] ComputeShader _computeShader = null;  // 計算用

    private Material _material;
    private ComputeBuffer _computeBuffer;   // コンピュートシェーダに渡すバッファ

    [SerializeField]
    private GameObject _fireball = null;   // 火球オブジェクト


    private void OnDestroy()
    {
        SafeRelease(_computeBuffer);
    }

    private void Start()
    {
        _material = new Material(_renderingShader);
        InitializeComputeBuffer();
    }

    private void Update()
    {
        Vector3 fireballPos = _fireball.transform.position;

        _computeShader.SetBuffer(0, "_Balls", _computeBuffer);
        _computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        _computeShader.SetVector("_Center", new Vector4(fireballPos.x, fireballPos.y, fireballPos.z, 1));
        _computeShader.Dispatch(0, _computeBuffer.count / 8 + 1, 1, 1);
    }

    private void InitializeComputeBuffer()
    {
        _computeBuffer = new ComputeBuffer(1000, Marshal.SizeOf(typeof(Fireball)));

        Fireball[] balls = new Fireball[_computeBuffer.count];
        for (int i = 0; i < _computeBuffer.count; i++)
        {
            Vector3 pos = (Random.onUnitSphere * 3) + _fireball.transform.position;
            balls[i] = new Fireball(
                pos,
                pos,
                new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)),
                new Color(1, 0, 0, 1)
                );
        }

        // バッファに適用  
        _computeBuffer.SetData(balls);
    }

    private void OnRenderObject()
    {
        // テクスチャ・バッファをマテリアルに設定
        _material.SetTexture("_MainTex", _texture);
        _material.SetBuffer("_Balls", _computeBuffer);

        _material.SetPass(0);

        Graphics.DrawProcedural(MeshTopology.Points, _computeBuffer.count);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        SafeRelease(_computeBuffer);
        InitializeComputeBuffer();
    }

    public void Stop()
    {
        gameObject.SetActive(false);
        _computeBuffer.Release();
    }

    private void SafeRelease(ComputeBuffer cb)
    {
        if (cb != null)
        {
            cb.Release();
        }
    }
}
