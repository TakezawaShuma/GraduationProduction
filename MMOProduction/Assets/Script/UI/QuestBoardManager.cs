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

    [SerializeField, Header("制限時間テキスト")]
    private Text timeLimitText = null;

    // 現在選択されているクエストID
    private MapID currentID = MapID.Non;

    private void Start()
    {
        panel.SetActive(false);

        marker.SetFunction(Open);
    }

    public void DecisionMapID()
    {
        if (currentID != MapID.Non)
        {
            var colors = toggleGroup.ActiveToggles().FirstOrDefault().colors;
            colors.normalColor = new Color(1, 1, 0, 1);
            toggleGroup.ActiveToggles().FirstOrDefault().colors = colors;
            UserRecord.MapID = currentID;
            Debug.Log(currentID + "に決定した");
        }
    }

    public void CancelMapID()
    {
        var colors = toggleGroup.ActiveToggles().FirstOrDefault().colors;
        colors.normalColor = new Color(1, 1, 1, 1);
        toggleGroup.ActiveToggles().FirstOrDefault().colors = colors;
        UserRecord.MapID = MapID.Base;
        Debug.Log("取り消した");
    }

    public void SetMapID()
    {
        if (toggleGroup.ActiveToggles().FirstOrDefault() != null)
        {
            currentID = toggleGroup.ActiveToggles().FirstOrDefault().gameObject.GetComponent<QuestToggle>().GetMapID();
        }
        else
        {
            currentID = MapID.Non;
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
}
