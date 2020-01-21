using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestButton : MonoBehaviour
{
    [SerializeField, Header("画像")]
    private Image image = null;

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
    }

    public void Decision()
    {
        image.enabled = true;
    }

    public void Cancel()
    {
        image.enabled = false;
    }
}
