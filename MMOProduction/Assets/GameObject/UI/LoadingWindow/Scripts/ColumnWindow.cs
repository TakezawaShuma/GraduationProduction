//
// ColumnWIndow.cs
//
// Author: Tamamura Shuuki
//

using System.Collections.Generic;
using UnityEngine;


// ロード中に表示するコラム画面
public class ColumnWindow
{

    List<Sprite> _images;       // コラム画像のリスト


    public ColumnWindow(string directoryPass)
    {
        _images = new List<Sprite>();

        Sprite[] imgs = Resources.LoadAll<Sprite>(directoryPass);
        foreach (var img in imgs)
        {
            _images.Add(img);
        }
    }

    public Sprite GetRandomImage()
    {
        int max = _images.Count;
        int index = Random.Range(0, max);
        Sprite o = _images[index];
        return o;
    }
}
