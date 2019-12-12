using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの名前を表示
/// </summary>
public class NameUI : MonoBehaviour
{
    [SerializeField]
    private Text text = null;

    public Text TEXT
    {
        get { return text; }
    }

    public void NameSet(string name)
    {
        text.text = name;
    }
}
