using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPCとの会話用クラス？
/// </summary>
public class Talk : MonoBehaviour
{
    // なんのオブジェクトなのかわからない？
    [SerializeField]
    private GameObject talkPanel = null;

    [SerializeField]
    private GameObject talkCamera = null;

    [SerializeField]
    private Marker marker = null;

    [SerializeField, TextArea(1, 5), Header("会話")]
    private string[] texts = null;

    [SerializeField, Header("テキスト")]
    private Text text = null;

    int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);

        talkPanel.SetActive(false);
        talkCamera.SetActive(false);

        if (text != null) text.text = texts[currentPage];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        talkPanel.SetActive(true);
        talkCamera.SetActive(true);
    }

    public void NextPage()
    {
        currentPage++;

        if(currentPage >= texts.Length)
        {
            currentPage = 0;

            talkPanel.SetActive(false);
            talkCamera.SetActive(false);
        }

        text.text = texts[currentPage];
    }
}
