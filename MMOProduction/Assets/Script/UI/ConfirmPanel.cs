using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField, Header("はいボタン")]
    private Button yesButton = null;

    public Button Yes
    {
        get { return yesButton; }
    }

    [SerializeField, Header("いいえボタン")]
    private Button noButton = null;

    public Button No
    {
        get { return noButton; }
    }

    [SerializeField, Header("テキスト")]
    private Text text = null;

    public Text Text
    {
        get { return text; }
    }
}
