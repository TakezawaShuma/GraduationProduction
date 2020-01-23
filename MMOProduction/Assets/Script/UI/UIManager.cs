using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// UI全般を管理するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ui = null;

    [SerializeField]
    private KeyCode[] key = null;

    [SerializeField]
    private UIShortCut[] shortcut = null;

    [SerializeField]
    private ChatController chat = null;

    [SerializeField]
    private Canvas canvas = null;
    // Start is called before the first frame update
    void Start()
    {
        //読み込む画像
        var sprites = Resources.LoadAll<Sprite>("Image/UIIcon");

        shortcut = new UIShortCut[ui.Length];

        GameObject parent = new GameObject();
        parent.name = "UIShortcut";
        parent.transform.SetParent(canvas.transform);
        if (ui != null)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(false);
                shortcut[i] = CreateButton(i,sprites).GetComponent<UIShortCut>();
                shortcut[i].transform.SetParent(parent.transform);
            }
        }

        AlignmentButton(shortcut);
    }

    private GameObject CreateButton(int num,Sprite[] sprite)
    {
        //UIショートカット用オブジェクト生成
        GameObject obj = new GameObject();
        obj.name = "UIshortcut" + num;

        //必要そうなコンポーネント追加
        var canvasRenderer = obj.AddComponent<CanvasRenderer>().GetComponent<CanvasRenderer>();
        var image = obj.AddComponent<Image>().GetComponent<Image>();
        var button = obj.AddComponent<Button>().GetComponent<Button>();
        var number = obj.AddComponent<UIShortCut>().GetComponent<UIShortCut>();

        //画像を設定する
        int spriteNumber = num*3;
        image.sprite = sprite[spriteNumber];

        button.targetGraphic = image;
        button.transition = Selectable.Transition.SpriteSwap;

        SpriteState spriteState = new SpriteState();
        spriteState.highlightedSprite = sprite[spriteNumber];
        spriteState.pressedSprite = sprite[spriteNumber+1];
        spriteState.disabledSprite = sprite[spriteNumber+2];

        button.spriteState = spriteState;

        // 関数の設定
        button.onClick.AddListener(number.OnClick);

        return obj;
    }

    private void AlignmentButton(UIShortCut[] obj)
    {
        int num = obj.Length - 1;
        Vector3 pos = new Vector3(1850, 60, 0);

        for (int i = num; i >= 0; i--)
        {
            obj[i].transform.position = pos;
            pos.x -= 110;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        InputManager.Update();

        if (ui != null && !chat.IsChatActive())
        {
            for (int i = 0; i < ui.Length; i++)
            {
                if (shortcut[i].state)
                {
                    if (ui[i].activeInHierarchy)
                    {
                        ui[i].SetActive(false);
                    }
                    else
                    {
                        ui[i].SetActive(true);
                        ui[i].transform.SetAsLastSibling();
                    }
                    shortcut[i].state = false;
                    return;
                }

                if (InputManager.InputKeyCheckDown(key[i]))
                {
                    if (ui[i].activeInHierarchy)
                    {
                        ui[i].SetActive(false);
                    }
                    else
                    {
                        ui[i].SetActive(true);
                        ui[i].transform.SetAsLastSibling();
                    }
                    return;
                }
            }
        }
    }

    public ChatController GetChat()
    {
        return chat;
    }
}
