//
// SwordTrailCreator.cs
//
// 剣の軌跡を描くメッシュを作成するクラス
//

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SwordTrailCreator : MonoBehaviour
{
    /// <summary>
    /// 頂点データ
    /// </summary>
    public struct Vertex
    {
        public Vector3 pos;
        public Vector2 uv;

        public Vertex(Vector3 pos, Vector2 uv)
        {
            this.pos = pos;
            this.uv = uv;
        }
    }


    // 軌跡用四角形メッシュの表示個数
    private static readonly int CREATE_NUM = 10;


    // 軌跡描画用の変換データ
    [SerializeField]
    private Transform _startPoint = null;   // 剣元
    [SerializeField]
    private Transform _endPoint = null;     // 剣先
    [SerializeField]
    private Transform _parent = null;       // このエフェクトの親オブジェクト

    // 頂点データリスト
    private List<Vertex> _vertices;
    // 頂点インデックスリスト
    private List<int> _indices;

    // 剣先の座標リスト
    private List<Vector3> _startPoints;     // 剣元
    private List<Vector3> _endPoints;       // 剣先

    // メッシュ描画用
    private Mesh _mesh;


    private void Start()
    {
        // 各リストの作成
        _vertices = new List<Vertex>();
        _indices = new List<int>();
        _startPoints = new List<Vector3>();
        _endPoints = new List<Vector3>();

        // 各種コンポーネント
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    private void LateUpdate()
    {
        // 座標のリセット（メッシュの生成位置をずらさないため）
        //this.transform.localScale = new Vector3(
        //    1 / _parent.localScale.x,
        //    1 / _parent.localScale.y,
        //    1 / _parent.localScale.z
        //    );
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        //this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.transform.position = new Vector3(0, 0, 0);
        //this.transform.localPosition = new Vector3(0, 0, 0);

        // 必要頂点数を超えたら削除
        if (_startPoints.Count >= CREATE_NUM + 1)
        {
            _startPoints.RemoveAt(0);
            _endPoints.RemoveAt(0);
        }

        // 現在の剣の位置を登録
        _startPoints.Add(_startPoint.position);
        _endPoints.Add(_endPoint.position);

        // 頂点がメッシュ数+1になったら斬撃用メッシュを作成
        if (_startPoints.Count >= CREATE_NUM + 1)
        {
            CreateSwordTrailMesh();
        }

        //Debug.Log("頂点データ数 : " + _vertices.Count);
    }

    /// <summary>
    /// 斬撃エフェクト用メッシュを作成する
    /// </summary>
    private void CreateSwordTrailMesh()
    {
        // メッシュのクリア
        _mesh.Clear();

        // リストのクリア
        _vertices.Clear();
        _indices.Clear();

        float uvParam = 0f;
        for (int i = 0; i < CREATE_NUM; i++)
        {
            // 頂点データ配列を作成し、リストに登録
            Vertex[] newVertices = new Vertex[]
            {
                new Vertex(_startPoints[i], new Vector2(uvParam, 0f)),
                new Vertex(_endPoints[i], new Vector2(uvParam, 1f)),
                new Vertex(_startPoints[i + 1], new Vector2(uvParam + 1f / CREATE_NUM, 0f)),
                new Vertex(_startPoints[i + 1], new Vector2(uvParam + 1f / CREATE_NUM, 0f)),
                new Vertex(_endPoints[i], new Vector2(uvParam, 1f)),
                new Vertex(_endPoints[i + 1], new Vector2(uvParam + 1f / CREATE_NUM, 1f)),
            };
            _vertices.AddRange(newVertices);

            // 表示する四角形数で1を割って割合を加算
            uvParam += 1f / CREATE_NUM;
        }

        // メッシュ用の三角形を登録した頂点で設定
        Vector3[] pos = new Vector3[_vertices.Count];
        Vector2[] uv = new Vector2[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++)
        {
            pos[i] = _vertices[i].pos;
            uv[i] = _vertices[i].uv;
            _indices.Add(i);
        }

        _mesh.vertices = pos;
        _mesh.uv = uv;
        _mesh.triangles = _indices.ToArray();

        // 再計算
        _mesh.RecalculateBounds();
        _mesh.RecalculateNormals();
    }
}
