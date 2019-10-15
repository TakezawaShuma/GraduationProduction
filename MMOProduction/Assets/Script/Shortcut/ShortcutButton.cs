using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShortcutButton : MonoBehaviour
{
    private Button button;
    private Image image;

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

    public void SetShortcut(UnityAction unityAction, Sprite sprite)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unityAction);

        if (sprite)
        {
            image.sprite = sprite;
        }
    }
}
