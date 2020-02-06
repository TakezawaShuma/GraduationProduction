//
// GrassField.cs
//
// Author: Tama
//

using System.Runtime.InteropServices;
using UnityEngine;

public class GrassField : MonoBehaviour
{

    public struct Buffer
    {
        public Vector3 startPos;
        public Vector3 pos;
        public Vector3 scale;
        public Vector3 wind;
        public int type;

        public Buffer(Vector3 startPos, Vector3 pos, Vector3 scale, Vector3 wind, int type)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.scale = scale;
            this.wind = wind;
            this.type = type;
        }
    }


    [SerializeField] Shader _renderingShader = null;       // 描画用シェーダ
    [SerializeField] ComputeShader _computeShader = null;  // 計算用

    private Material _material;
    private ComputeBuffer _computeBuffer;   // コンピュートシェーダに渡すバッファ

    [SerializeField]
    private double _generateNum = 100;

    [SerializeField]
    private Transform _center = null;

    [SerializeField]
    private float _range = 100f;

    [SerializeField]
    private Vector3 _offsetPos = new Vector3(0, 0, 0);

    [System.Serializable]
    public struct Property
    {
        public Texture texure;
        public Texture heightMap;
        public Texture rampTex;
        public Color shadowColor;
        public Vector3 scale;
        public Vector3 translation;
        public Vector2 heightMapPos;
        public float heightMapScale;
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
        _material.renderQueue = 0;
        InitializeComputeBuffer();
    }

    private void Update()
    {
        _computeShader.SetBuffer(0, "_Grass", _computeBuffer);
        _computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        _computeShader.SetVector("_Center", _center.position);
        _computeShader.Dispatch(0, 1, 1, 1);
    }

    private void InitializeComputeBuffer()
    {
        // コンピュートバッファ作成
        _computeBuffer = new ComputeBuffer((int)_generateNum, Marshal.SizeOf(typeof(Buffer)));

        Buffer[] buff = new Buffer[_computeBuffer.count];
        for (int i = 0; i < _computeBuffer.count; i++)
        {
            Vector3 startPos = new Vector3(Random.Range(-_range, _range), 0, Random.Range(-_range, _range)) + _offsetPos;
            buff[i] = new Buffer(
                startPos,
                startPos,
                new Vector3(1, Random.Range(1f, 2f), 1),
                new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z),
                Random.Range(0, 4)
                );
        }

        // バッファに適用  
        _computeBuffer.SetData(buff);
    }

    private void OnRenderObject()
    {
        if (Camera.current.tag == "MainCamera")
        {
            // テクスチャ・バッファをマテリアルに設定
            _material.SetTexture("_MainTex", _property.texure);
            _material.SetTexture("_HeightMap", _property.heightMap);
            _material.SetTexture("_RampTex", _property.rampTex);
            _material.SetColor("_ShadowColor", _property.shadowColor);
            _material.SetVector("_Scale", _property.scale);
            _material.SetVector("_Translation", _property.translation);
            _material.SetVector("_HeightMapPoint", _property.heightMapPos);
            _material.SetFloat("_HeightMapScale", _property.heightMapScale);
            _material.SetBuffer("_Grass", _computeBuffer);

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
