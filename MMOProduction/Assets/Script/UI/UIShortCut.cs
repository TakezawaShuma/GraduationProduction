using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShortCut : MonoBehaviour
{
    public bool state { get; set; }

    public void OnClick()
    {
        state = true;
    }
}
