using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTalk : MonoBehaviour
{
    // なんのオブジェクトなのかわからない？
    [SerializeField]
    private GameObject[] gameObject_ = null;

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

        foreach (GameObject game in gameObject_)
        {
            game.SetActive(false);
        }

        text.text = texts[currentPage];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        foreach (GameObject game in gameObject_)
        {
            game.SetActive(true);
        }
    }

    public void NextPage()
    {
        currentPage++;

        if(currentPage >= texts.Length)
        {
            currentPage = 0;

            foreach (GameObject game in gameObject_)
            {
                game.SetActive(false);
            }
        }

        text.text = texts[currentPage];
    }
}
