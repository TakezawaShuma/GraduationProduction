//
// ForestGenerator.cs
//
// Author: Tama
//
// 木を配置して森を作る
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject _treePrefab = null;

    [SerializeField]
    private Texture2D _generateMap = null;


    private void Start()
    {
        //tree.transform.localPosition = new Vector3(Random.Range(-125, 125), 0, Random.Range(-125, 125));
        Color[] pixels = _generateMap.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (i % 8 != 0)
            {
                if (pixels[i].r >= 0.9)
                {
                    GameObject tree = Instantiate(_treePrefab, this.transform);
                    tree.transform.localScale = new Vector3(5, 5, 5);
                    int x = (i % (int)_generateMap.width) * 4;
                    int z = (i / (int)_generateMap.height) * 4;
                    tree.transform.localPosition = new Vector3(x + 125, 10, z - 125);
                }
            } 
        }
    }
}
