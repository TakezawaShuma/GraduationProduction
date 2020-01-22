using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMapMoveImage : MonoBehaviour
{
    [SerializeField, Header("画像")]
    private List<Image> images = null;

    [SerializeField, Header("何秒間で画像が切り替わるか")]
    private float changeTime = 1f;

    private float currentTime = 0f;

    private int count = 0;

    private bool state = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < images.Count; i++)
        {
            images[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state)
        {
            currentTime += Time.deltaTime;

            if (currentTime > changeTime)
            {
                currentTime = 0;

                if (count < images.Count)
                {
                    images[count].enabled = true;
                    count++;
                }
                else
                {
                    for (int i = 0; i < images.Count; i++)
                    {
                        images[i].enabled = false;
                    }

                    count = 0;
                }
            }
        }
        else
        {
            currentTime = 0;
            for (int i = 0; i < images.Count; i++)
            {
                images[i].enabled = false;
            }
        }
    }

    public void SetState(bool state)
    {
        this.state = state;
    }
}
