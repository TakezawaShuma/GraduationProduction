using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
