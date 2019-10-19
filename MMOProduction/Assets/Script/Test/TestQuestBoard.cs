using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuestBoard : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject;

    [SerializeField]
    private Marker marker;

    // Start is called before the first frame update
    void Start()
    {
        marker.SetFunction(On);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}
