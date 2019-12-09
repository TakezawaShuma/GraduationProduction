using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInitialize : MonoBehaviour
{
    [SerializeField]
    private int width = 300;
    [SerializeField]
    private int height = 300; 

    [SerializeField]
    private bool RightScrollbarFlag;
    [SerializeField]
    private bool ButtomScrollbarFlag;

    // Start is called before the first frame update
    void Start()
    {
        

        Vector2 rect = new Vector2(width, height);
        //自分の奴
        this.GetComponent<RectTransform>().sizeDelta = rect;

        rect.x = rect.x - 8;
        rect.y = rect.y - 8;

        var backImage = this.transform.GetChild(0);
        backImage.GetComponent<RectTransform>().sizeDelta = rect;

        rect.x = rect.x - 16;
        rect.y = rect.y - 66;

        backImage.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(rect.x, 50.0f);
        backImage.GetChild(1).GetComponent<RectTransform>().sizeDelta = rect;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
