using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkilltreeManager : MonoBehaviour
{
    private enum Buttons
    {
        AllJob,
        ShieldSword,
        TwoHandedSword,
        Katana,
        Fist,
        Spear,
        DualBlades,
        Bow,
        MagicWand,
        Grimoire,
        HolyWand,
    }

    [SerializeField]
    private GameObject[] panel = new GameObject[11];

    [SerializeField]
    private Button[] buttons = new Button[11];

    [SerializeField]
    private Color onColor = default(Color);

    [SerializeField]
    private Color offColor = default(Color);

    [SerializeField]
    private ScrollRect scrollRect = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AllFalse()
    {
        foreach(GameObject gameObject in panel)
        {
            gameObject.SetActive(false);
        }

        foreach (Button button in buttons)
        {
            button.colors = OffColor(button);
        }
    }

    private ColorBlock OnColor(Button button)
    {
        ColorBlock color = button.colors;
        color.normalColor = onColor;
        color.highlightedColor = onColor;
        return color;
    }

    private ColorBlock OffColor(Button button)
    {
        ColorBlock color = button.colors;
        color.normalColor = offColor;
        color.highlightedColor = offColor;
        return color;
    }

    public void On()
    {
        this.gameObject.SetActive(true);
    }

    public void Off()
    {
        this.gameObject.SetActive(false);
    }

    public void AllJob()
    {
        AllFalse();
        panel[(int)Buttons.AllJob].SetActive(true);
        buttons[(int)Buttons.AllJob].colors = OnColor(buttons[(int)Buttons.AllJob]);
        scrollRect.content = panel[(int)Buttons.AllJob].GetComponent<RectTransform>();
    }

    public void ShieldSword()
    {
        AllFalse();
        panel[(int)Buttons.ShieldSword].SetActive(true);
        buttons[(int)Buttons.ShieldSword].colors = OnColor(buttons[(int)Buttons.ShieldSword]);
        scrollRect.content = panel[(int)Buttons.ShieldSword].GetComponent<RectTransform>();
    }

    public void TwoHandedSword()
    {
        AllFalse();
        panel[(int)Buttons.TwoHandedSword].SetActive(true);
        buttons[(int)Buttons.TwoHandedSword].colors = OnColor(buttons[(int)Buttons.TwoHandedSword]);
        scrollRect.content = panel[(int)Buttons.TwoHandedSword].GetComponent<RectTransform>();
    }

    public void Katana()
    {
        AllFalse();
        panel[(int)Buttons.Katana].SetActive(true);
        buttons[(int)Buttons.Katana].colors = OnColor(buttons[(int)Buttons.Katana]);
        scrollRect.content = panel[(int)Buttons.Katana].GetComponent<RectTransform>();
    }

    public void Fist()
    {
        AllFalse();
        panel[(int)Buttons.Fist].SetActive(true);
        buttons[(int)Buttons.Fist].colors = OnColor(buttons[(int)Buttons.Fist]);
        scrollRect.content = panel[(int)Buttons.Fist].GetComponent<RectTransform>();
    }

    public void Spear()
    {
        AllFalse();
        panel[(int)Buttons.Spear].SetActive(true);
        buttons[(int)Buttons.Spear].colors = OnColor(buttons[(int)Buttons.Spear]);
        scrollRect.content = panel[(int)Buttons.Spear].GetComponent<RectTransform>();
    }

    public void DualBlades()
    {
        AllFalse();
        panel[(int)Buttons.DualBlades].SetActive(true);
        buttons[(int)Buttons.DualBlades].colors = OnColor(buttons[(int)Buttons.DualBlades]);
        scrollRect.content = panel[(int)Buttons.DualBlades].GetComponent<RectTransform>();
    }

    public void Bow()
    {
        AllFalse();
        panel[(int)Buttons.Bow].SetActive(true);
        buttons[(int)Buttons.Bow].colors = OnColor(buttons[(int)Buttons.Bow]);
        scrollRect.content = panel[(int)Buttons.Bow].GetComponent<RectTransform>();
    }

    public void MagicWand()
    {
        AllFalse();
        panel[(int)Buttons.MagicWand].SetActive(true);
        buttons[(int)Buttons.MagicWand].colors = OnColor(buttons[(int)Buttons.MagicWand]);
        scrollRect.content = panel[(int)Buttons.MagicWand].GetComponent<RectTransform>();
    }

    public void Grimoire()
    {
        AllFalse();
        panel[(int)Buttons.Grimoire].SetActive(true);
        buttons[(int)Buttons.Grimoire].colors = OnColor(buttons[(int)Buttons.Grimoire]);
        scrollRect.content = panel[(int)Buttons.Grimoire].GetComponent<RectTransform>();
    }

    public void HolyWand()
    {
        AllFalse();
        panel[(int)Buttons.HolyWand].SetActive(true);
        buttons[(int)Buttons.HolyWand].colors = OnColor(buttons[(int)Buttons.HolyWand]);
        scrollRect.content = panel[(int)Buttons.HolyWand].GetComponent<RectTransform>();
    }
}
