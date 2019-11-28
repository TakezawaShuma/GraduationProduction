using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIの×ボタン
/// </summary>
public class Close : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickClose()
    {
        var par = this.transform.parent;
        par.gameObject.SetActive(false);
    }
}
