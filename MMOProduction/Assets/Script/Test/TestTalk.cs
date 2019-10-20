using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTalk : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObject;

    [SerializeField]
    private Marker marker;

    [SerializeField, TextArea(1, 5), Header("会話")]
    private string[] texts;

    [SerializeField, Header("テキスト")]
    private Text text;

    int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);

        foreach (GameObject game in gameObject)
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
        foreach (GameObject game in gameObject)
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

            foreach (GameObject game in gameObject)
            {
                game.SetActive(false);
            }
        }

        text.text = texts[currentPage];
    }
}
