using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestBoard : MonoBehaviour
{
    // なんの変数なのかわからない
    [SerializeField]
    private GameObject[] gameObject_ = null;

    [SerializeField]
    private Marker marker = null;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);

        foreach (GameObject game in gameObject_)
        {
            game.SetActive(false);
        }
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

    public void Off()
    {
        foreach (GameObject game in gameObject_)
        {
            game.SetActive(false);
        }
    }
}
