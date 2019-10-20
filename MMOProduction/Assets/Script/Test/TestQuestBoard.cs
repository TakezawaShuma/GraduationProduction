using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObject;

    [SerializeField]
    private Marker marker;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);

        foreach (GameObject game in gameObject)
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
        foreach (GameObject game in gameObject)
        {
            game.SetActive(true);
        }
    }

    public void Off()
    {
        foreach (GameObject game in gameObject)
        {
            game.SetActive(false);
        }
    }
}
