//
// MiniFireEffect.cs
//

using System.Runtime.InteropServices;
using UnityEngine;

public class MiniFireEffect : MonoBehaviour
{

    /// <summary>
    /// ミニファイアに使用するパーティクルバッファ
    /// </summary>
    public struct Particle
    {
        public Vector3 pos;
        public Vector3 startPos;
        public Vector3 vel;
        public Color color;

        public Particle(Vector3 pos, Vector3 startPos, Vector3 vel, Color color)
        {
            this.pos = pos;
            this.startPos = startPos;
            this.vel = vel;
            this.color = color;
        }
    }


    [SerializeField] Shader _renderingShader = null;       // 描画用シェーダ
    [SerializeField] Texture _texture = null;
    [SerializeField] ComputeShader _computeShader = null;  // 計算用

    private Material _material;
    private ComputeBuffer _computeBuffer;   // コンピュートシェーダに渡すバッファ

    [SerializeField]
    private Transform _center = null;   // パーティクルたちの中心位置

    [SerializeField]
    private Color _color = new Color(1, 1, 1, 1);


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
        _computeShader.SetBuffer(0, "_Particle", _computeBuffer);
        _computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        _computeShader.SetVector("_Center", new Vector4(_center.position.x, _center.position.y, _center.position.z, 1));
        _computeShader.Dispatch(0, _computeBuffer.count / 8 + 1, 1, 1);
    }

    private void InitializeComputeBuffer()
    {
        _computeBuffer = new ComputeBuffer(100, Marshal.SizeOf(typeof(Particle)));

        Particle[] buff = new Particle[_computeBuffer.count];
        for (int i = 0; i < _computeBuffer.count; i++)
        {
            Vector3 pos = _center.position;
            //pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            buff[i] = new Particle(
                pos,
                pos,
                new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f)),
                _color
                );
        }

        // バッファに適用  
        _computeBuffer.SetData(buff);
    }

    private void OnRenderObject()
    {
        // テクスチャ・バッファをマテリアルに設定
        _material.SetTexture("_MainTex", _texture);
        _material.SetBuffer("_Particle", _computeBuffer);

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
