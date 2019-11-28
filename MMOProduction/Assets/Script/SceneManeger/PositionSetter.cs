////////////////////////////////////////////////////////
// タイトルシーンのUI等のポジションを設定するクラス？ //
////////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーンのボタン等の位置を設定するクラス
/// </summary>
public class PositionSetter : MonoBehaviour
{
    public enum PositionX
    {
        LEFT,
        CENTER,
        RIGHT,
        FREE
    };
    public enum PositionY
    {
        TOP,
        MIDDLE,
        BOTTOM,
        FREE
    };

    [SerializeField]
    public PositionX positionX = PositionX.FREE;
    [SerializeField]
    public PositionY positionY = PositionY.FREE;

    public bool isUpdate = false;

    public Vector2 margin;

    private RectTransform rectTrans;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdate) { SetPosition(); }
    }
    private void SetPosition()
    {
        Vector2 pos = rectTrans.anchoredPosition;
        switch (positionX)
        {
            case PositionX.CENTER:
                pos.x = 0f;
                break;
            case PositionX.LEFT:
                pos.x = -Screen.width * 0.5f + rectTrans.rect.width * 0.5f + margin.x;
                break;
            case PositionX.RIGHT:
                pos.x = Screen.width * 0.5f - rectTrans.rect.width * 0.5f - margin.x;
                break;
        }

        switch (positionY)
        {
            case PositionY.MIDDLE:
                pos.y = 0f;
                break;
            case PositionY.BOTTOM:
                pos.y = -Screen.height * 0.5f + rectTrans.rect.height * 0.5f + margin.y;
                break;
            case PositionY.TOP:
                pos.y = Screen.height * 0.5f - rectTrans.rect.height * 0.5f - margin.y;
                break;
        }
        rectTrans.anchoredPosition = pos;
    }
}
