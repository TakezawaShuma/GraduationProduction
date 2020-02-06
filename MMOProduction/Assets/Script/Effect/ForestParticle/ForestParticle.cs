//
// ForestParticle.cs
//
// Author: Tama
//
// 森の中を漂う粒子
//

using System.Runtime.InteropServices;
using UnityEngine;

public class ForestParticle : MonoBehaviour
{

    public struct Buffer
    {
        public Vector3 startPos;
        public Vector3 pos;
        public Vector3 scale;
        public Vector3 vel;
        public float life;

        public Buffer(Vector3 startPos, Vector3 pos, Vector3 scale, Vector3 vel, float life)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.scale = scale;
            this.vel = vel;
            this.life = life;
        }
    }


    [SerializeField] Shader _renderingShader = null;       // 描画用シェーダ
    [SerializeField] ComputeShader _computeShader = null;  // 計算用

    private Material _material;
    private ComputeBuffer _computeBuffer;   // コンピュートシェーダに渡すバッファ

    [SerializeField]
    private double _generateNum = 100;

    [SerializeField]
    private float _range = 100f;

    [SerializeField]
    private Vector3 _offsetPos = new Vector3(0, 0, 0);

    [System.Serializable]
    public struct Property
    {
        public Texture texure;
        public Texture rampTex;
        public Color color;
    }
    [SerializeField]
    private Property _property;


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
        _computeShader.SetFloat("_Time", Time.time);
        _computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        _computeShader.Dispatch(0, _computeBuffer.count / 8 + 1, 1, 1);
    }

    private void InitializeComputeBuffer()
    {
        // コンピュートバッファ作成
        _computeBuffer = new ComputeBuffer((int)_generateNum, Marshal.SizeOf(typeof(Buffer)));

        Buffer[] buff = new Buffer[_computeBuffer.count];
        for (int i = 0; i < _computeBuffer.count; i++)
        {
            Vector3 startPos = new Vector3(Random.Range(-_range, _range), Random.Range(0f, 100f), Random.Range(-_range, _range)) + _offsetPos;
            buff[i] = new Buffer(
                startPos,
                startPos,
                new Vector3(0.05f, 1, 1),
                new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)),
                Random.Range(0.1f, 1f)
                );
        }

        // バッファに適用  
        _computeBuffer.SetData(buff);
    }

    private void OnRenderObject()
    {
        // カメラがメインカメラの場合に描画を行う
        if (Camera.current.tag == "MainCamera")
        {
            // テクスチャ・バッファをマテリアルに設定
            _material.SetTexture("_MainTex", _property.texure);
            _material.SetTexture("_RampTex", _property.rampTex);
            _material.SetColor("_Color", _property.color);
            _material.SetBuffer("_Particle", _computeBuffer);

            _material.SetPass(0);

            Graphics.DrawProcedural(MeshTopology.Points, _computeBuffer.count);
        }
    }

    private void SafeRelease(ComputeBuffer cb)
    {
        if (cb != null)
        {
            cb.Release();
        }
    }
}
