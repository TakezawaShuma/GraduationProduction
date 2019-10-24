using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveRange : MonoBehaviour
{
    Canvas canvas;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;

        float h = image.preferredHeight / 2;
        float w = image.preferredWidth / 2;

        Rect rect = canvas.pixelRect;

        if (rect.height < y + h / 2)
        {
            this.transform.position = new Vector3(rect.height-h, y, 0);
        }
    }
}
