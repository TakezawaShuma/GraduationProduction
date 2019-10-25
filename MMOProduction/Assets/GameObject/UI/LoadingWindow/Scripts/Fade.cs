//
// Fade.cs
// 
// Author: Tamamura Shuuki
//

using UnityEngine;


// フェードイン・アウトを適用
[ExecuteInEditMode]
public class Fade : MonoBehaviour
{

    [SerializeField]
    private Material _material = null;

    [SerializeField, Range(0, 1)]
    private float _param = 0;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _material.SetFloat("_Param", _param);
        Graphics.Blit(source, destination, _material);
    }
}
