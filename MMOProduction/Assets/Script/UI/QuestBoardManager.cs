﻿using System.Collections;
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

    // 現在選択されているクエスト
    private Packes.QuestMasterData currentQuest;

    private List<Packes.QuestMasterData> datas;

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

            count++;
        }

        currentQuest = new Packes.QuestMasterData();
    }

    public void DecisionMapID()
    {
        if (currentQuest.id != 0)
        {
            var colors = toggleGroup.ActiveToggles().FirstOrDefault().colors;
            colors.normalColor = new Color(1, 1, 0, 1);
            toggleGroup.ActiveToggles().FirstOrDefault().colors = colors;
            UserRecord.NextMapId = (MapID)currentQuest.mapId;
            UserRecord.QuestID = currentQuest.id;
            Debug.Log((MapID)currentQuest.mapId + "に決定した");
            WS.WsPlay.Instance.Send(new Packes.QuestOrder(UserRecord.ID, currentQuest.id).ToJson());
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
}
