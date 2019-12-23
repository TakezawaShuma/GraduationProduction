//
// SnowFallEffect.cs
//
// Author : Tama
//
// 降雪を表現するパーティクルエフェクト
//

using UnityEngine;
using System.Runtime.InteropServices;

public class SnowFallEffect : MonoBehaviour
{

    // シェーダーに渡すバッファ
    public struct Particle
    {
        public Vector3 startPos;
        public Vector3 pos;
        public Vector3 vel;
        public Color col;
        public float life;

        public Particle(Vector3 startPos, Vector3 pos, Vector3 vel, Color col, float life)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.vel = vel;
            this.col = col;
            this.life = life;
        }
    }

    // シェーダープロパティ
    [System.Serializable]
    public struct Property
    {
        public Texture texture;
        public float gravity;
    }

    [SerializeField] Property _property;
    [SerializeField] Shader _renderingShader = null;       // 描画用シェーダ
    [SerializeField] ComputeShader _computeShader = null;  // 計算用
    //[SerializeField] Transform _owner = null;

    private Material _material;
    private ComputeBuffer _computeBuffer;   // コンピュートシェーダに渡すバッファ

    [SerializeField] int _maxParticles = 1000; // エフェクト最大生成数
    [SerializeField] Color _color = Color.white;    // パーティクルの色

    [SerializeField] Transform _target = null;     // 雪を降らせるターゲット
    [SerializeField] float _range = 250;    // 雪を降らせる範囲
    [SerializeField] float _height = 250;   // 雪を降らせる高さ


    private void Start()
    {
        _maxParticles = Mathf.Max(0, _maxParticles);

        _material = new Material(_renderingShader);
        InitializeComputeBuffer();
    }

    private void Update()
    {
        UpdateComputeBuffer();
    }

    private void OnRenderObject()
    {
        // テクスチャ・バッファをマテリアルに設定
        _material.SetBuffer("_Particle", _computeBuffer);
        _material.SetTexture("_MainTex", _property.texture);

        _material.SetPass(0);

        Graphics.DrawProcedural(MeshTopology.Points, _computeBuffer.count);
    }

    private void OnDestroy()
    {
        // コンピュートバッファを開放する
        SafeRelease(_computeBuffer);
    }

    // --------------------------------------------
    // コンピュートバッファの初期化処理
    // --------------------------------------------
    private void InitializeComputeBuffer()
    {
        // コンピュートバッファを作成する
        _computeBuffer = new ComputeBuffer(_maxParticles, Marshal.SizeOf(typeof(Particle)));

        Particle[] particle = new Particle[_computeBuffer.count];
        for (int i = 0; i < _computeBuffer.count; i++)
        {
            // データを生成して配列に格納
            Vector3 startPos = new Vector3(Random.Range(-250f, 250f), Random.Range(0f, 250f), Random.Range(-250f, 250f));
            //Vector3 startPos = new Vector3(Random.Range(-_range, _range), Random.Range(0, _height), Random.Range(-_range, _range));
            //startPos += _target.transform.position;
            particle[i] = new Particle(
                startPos,
                startPos,
                new Vector3(Random.Range(-1f, 1f), -Random.Range(1f, 3f), Random.Range(-1f, 1f)),
                _color,
                1
                ); ;
        }

        // バッファに適用  
        _computeBuffer.SetData(particle);
    }

    // --------------------------------------------
    // コンピュートバッファの更新処理
    // --------------------------------------------
    private void UpdateComputeBuffer()
    {
        _computeShader.SetBuffer(0, "_Particle", _computeBuffer);
        _computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        _computeShader.SetFloat("_Gravity", _property.gravity);
        _computeShader.Dispatch(0, _computeBuffer.count / 8 + 1, 1, 1);
    }

    // --------------------------------------------
    // コンピュートバッファを安全に開放する
    // --------------------------------------------
    private void SafeRelease(ComputeBuffer cb)
    {
        if (cb != null)
            cb.Release();
    }
}
