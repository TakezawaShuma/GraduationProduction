using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    private bool flag = false;
    public bool FLAG
    {
        set { flag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            sprite.color = new Color(1, 0, 0, 1);
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}
