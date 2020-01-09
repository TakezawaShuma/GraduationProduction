using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        if (time < 10)
        {
            alpha += 0.1f;
            pos.y++;
            
        }
        else if(time < 20)
        {
           //何もしない 
        }
        else if(time < 60)
        {
            alpha -= 0.05f;
            pos.y += 4;
        }
        else
        {
            Destroy(this.gameObject);
        }

        this.transform.position = pos;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            var color = this.transform.GetChild(i).GetComponent<Image>().color;
            color.a = alpha;
            this.transform.GetChild(i).GetComponent<Image>().color = color;
        }
        time++;
    }
}
