using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoardManager : MonoBehaviour
{
    [SerializeField, Header("パネル")]
    private GameObject panel = null;

    [SerializeField, Header("クエストボタン")]
    private List<Button> questButton = null;

    [SerializeField, Header("マーカー")]
    private Marker marker = null;

    // 現在選択されているクエストID
    private int currentID = -1;

    private void Start()
    {
        int i = 1;

        foreach(Button button in questButton)
        {
            button.onClick.AddListener(() => { ButtonMapID(i); });
            i++;
        }

        marker.SetFunction(Open);
    }

    public void DecisionMapID()
    {
        if (currentID != -1)
        {
            UserRecord.MapID = (MapID)currentID;
        }
    }

    private void ButtonMapID(int id)
    {
        currentID = id;
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
