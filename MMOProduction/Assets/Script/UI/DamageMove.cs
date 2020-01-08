using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMove : MonoBehaviour
{
    [SerializeField]
    private float alpha = 0;

    [SerializeField]
    private int time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = this.transform.position;

        if(time < 10)
        {
            alpha += 0.1f;
            pos.y++;
            
        }
        else if(time < 40)
        {
           //何もしない 
        }
        else if(time < 60)
        {
            alpha -= 0.05f;
            pos.y += 2;
        }
        else
        {
            Destroy(this.gameObject);
        }

        time++;
    }
}
