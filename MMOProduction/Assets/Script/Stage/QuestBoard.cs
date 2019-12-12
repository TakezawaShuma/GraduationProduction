using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クエストボード
/// </summary>
public class QuestBoard : StageObject
{
    //[SerializeField]
    //private Targetable targetMarker = null;

    // クエストパネル
    [SerializeField]
    private GameObject questPanel = null;

    [SerializeField]
    private Marker marker = null;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);

        questPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        questPanel.SetActive(true);
    }

    public void Off()
    {
        questPanel.SetActive(false);
    }
}
