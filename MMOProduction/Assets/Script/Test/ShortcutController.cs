using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutController : MonoBehaviour
{
    [SerializeField, Header("ショートカットの数")]
    private int MaxShortCut = 0;

    private KeyCode[,] ShortcutKeys;

    // Start is called before the first frame update
    void Start()
    {
        ShortcutKeys = new KeyCode[MaxShortCut, 2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
