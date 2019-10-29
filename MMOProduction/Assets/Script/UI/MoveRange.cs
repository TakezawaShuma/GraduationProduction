using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveRange : MonoBehaviour
{
    public GameObject canvas;
    private Rect canvasRect;
    private Rect thisRect;
    // Start is called before the first frame update
    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>().rect;
        thisRect = this.GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsIn(canvasRect, thisRect))
        {
            Vector3 vel = Vector3.Normalize(this.transform.position);
            this.transform.position = (this.transform.position + vel);
            Debug.Log(vel);
        }
    }

    //範囲内にあるかを判定する
    bool IsIn(Rect canvas, Rect image)
    {

        if (canvas.xMax < image.xMax) return false;
        if (canvas.xMin > image.xMin) return false;
        if (canvas.yMax < image.yMax) return false;
        if (canvas.yMin > image.yMin) return false;

        Debug.Log(canvas.xMax + image.xMax);


        Debug.Log(true);
        return true;
    }
}
