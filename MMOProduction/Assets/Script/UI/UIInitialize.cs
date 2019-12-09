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

        var titleImage = this.transform.GetChild(1);
        titleImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.x, 50.0f);

        var fieldImage = this.transform.GetChild(2);
        fieldImage.GetComponent<RectTransform>().sizeDelta = rect;

    }
}
