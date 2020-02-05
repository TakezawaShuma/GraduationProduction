using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestBoardManager : MonoBehaviour
{
    [SerializeField, Header("パネル")]
    private GameObject panel = null;

    [SerializeField, Header("トグルグループ")]
    private ToggleGroup toggleGroup = null;

    [SerializeField, Header("マーカー")]
    private Marker marker = null;

    [SerializeField, Header("クエスト内容テキスト")]
    private Text detailText = null;

    [SerializeField, Header("クエストトグルプレハブ")]
    private GameObject prefab = null;

    [SerializeField, Header("クエストトグル置くとこ")]
    private GameObject togglePutObj = null;

    [SerializeField, Header("確認ウィンドウ")]
    private ConfirmPanel confirmPanel = null;

    // 現在選択されているクエスト
    private Packes.QuestMasterData currentQuest;

    private List<Packes.QuestMasterData> datas;

    private List<GameObject> toggles = new List<GameObject>();

    private void Start()
    {
        panel.SetActive(false);

        marker.SetFunction(Open);

        datas = QuestDatas.GetAll();

        int count = 0;

        foreach(Packes.QuestMasterData data in datas)
        {
            GameObject gameObject = Instantiate(prefab, togglePutObj.transform);

            gameObject.transform.Translate(new Vector3(0, -40 * count, 0));

            gameObject.GetComponent<QuestToggle>().data = data;

            gameObject.GetComponent<QuestToggle>().GetText().text = data.name;

            gameObject.GetComponent<Toggle>().group = toggleGroup;

            gameObject.GetComponent<Toggle>().onValueChanged.AddListener(SetMapID);

            toggles.Add(gameObject);
            count++;
        }

        currentQuest = new Packes.QuestMasterData();

        CloseConfirm();
    }

    private void Update()
    {
        foreach(GameObject toggle in toggles)
        {
            Packes.QuestMasterData data = toggle.GetComponent<QuestToggle>().data;

            if(UserRecord.QuestID != data.id)
            {
                ToggleChangeColor(toggle.GetComponent<Toggle>(), 0);
            }
        }
    }

    public void OpenDecisionConfirm()
    {
        if (currentQuest.id != 0 || currentQuest.id != UserRecord.QuestID || UserRecord.QuestID == 0)
        {
            confirmPanel.gameObject.SetActive(true);
            confirmPanel.Text.text = currentQuest.name + "を受注しますか？";
            confirmPanel.Yes.onClick.AddListener(DecisionMapID);
        }
    }

    public void OpenCancelConfirm()
    {
        if (UserRecord.QuestID != 0)
        {
            confirmPanel.gameObject.SetActive(true);
            confirmPanel.Text.text = QuestDatas.FindOne(UserRecord.QuestID).name + "の受注を取り消しますか？";
            confirmPanel.Yes.onClick.AddListener(CancelMapID);
        }
    }

    public void CloseConfirm()
    {
        confirmPanel.gameObject.SetActive(false);
        confirmPanel.Yes.onClick.RemoveAllListeners();
        confirmPanel.Yes.onClick.AddListener(CloseConfirm);
    }

    public void DecisionMapID()
    {
        ToggleChangeColor(toggleGroup.ActiveToggles().FirstOrDefault(),1);
        UserRecord.NextMapId = (MapID)currentQuest.mapId;
        UserRecord.QuestID = currentQuest.id;
        WS.WsPlay.Instance.Send(new Packes.QuestOrder(UserRecord.ID, currentQuest.id).ToJson());
        Close();
    }

    public void CancelMapID()
    {
        ToggleChangeColor(toggleGroup.ActiveToggles().FirstOrDefault(),0);
        UserRecord.NextMapId = MapID.Non;
        UserRecord.QuestID = 0;
        Close();
    }

    public void SetMapID(bool b)
    {
        if (toggleGroup.ActiveToggles().FirstOrDefault() != null)
        {
            currentQuest = toggleGroup.ActiveToggles().FirstOrDefault().gameObject.GetComponent<QuestToggle>().data;
            detailText.text = toggleGroup.ActiveToggles().FirstOrDefault().gameObject.GetComponent<QuestToggle>().data.comment;
        }
        else
        {
            currentQuest = new Packes.QuestMasterData();
        }
    }

    public void Open()
    {
        panel.gameObject.SetActive(true);
    }

    public void Close()
    {
        panel.gameObject.SetActive(false);
    }

    private void ToggleChangeColor(Toggle toggle, int mode)
    {
        if (toggle == null) return;

        var colors = toggle.colors;
        switch (mode)
        {
            case 0:
                colors.normalColor = new Color(1, 1, 1, 1);
                colors.highlightedColor = new Color(1, 1, 1, 1);
                break;
            case 1:
                colors.normalColor = new Color(1, 1, 0, 1);
                colors.highlightedColor = new Color(1, 1, 0, 1);
                break;
            default:
                break;
        }
        toggle.colors = colors;
    }
}
