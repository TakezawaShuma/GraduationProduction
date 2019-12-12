using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 他プレイヤーの名前を表示
/// </summary>
public class OtherUserNameUI : MonoBehaviour
{
    [SerializeField, Header("テキスト")]
    private Text text = null;

    public string UserName
    {
        set { text.text = value; }
    }
}
