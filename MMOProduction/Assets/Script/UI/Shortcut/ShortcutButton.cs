using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShortcutButton : MonoBehaviour
{
    private Button button;

    [SerializeField, Header("テキスト")]
    private Text text = null;

    [SerializeField, Header("画像")]
    private Image image = null;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShortcut(UnityAction unityAction, string text, Sprite sprite)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unityAction);

        this.text.text = text;

        if (sprite)
        {
            image.sprite = sprite;
        }
    }
}
