using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public Player PLAYER { get; set; }

    string[] StatusNames;
    // Start is called before the first frame update
    void Start()
    {
        int count = this.transform.childCount;

        StatusNames = new string[count];

        for (int i = 0; i < count; i++)
        {
            StatusNames[i] = this.transform.GetChild(i).GetComponent<Text>().text;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PLAYER);
        int count = this.transform.childCount;

        string[] st = new string[count];

        st[0] = PLAYER.hp.ToString() + "/" + PLAYER.maxHp.ToString();
        st[1] = PLAYER.mp.ToString() + "/" + PLAYER.maxMp.ToString();
        st[2] = PLAYER.STR.ToString();
        st[3] = PLAYER.VIT.ToString();
        st[4] = PLAYER.INT.ToString();
        st[5] = PLAYER.MND.ToString();
        st[6] = PLAYER.DEX.ToString();
        st[3] = PLAYER.AGI.ToString();
        Debug.Log(
            "str : " + PLAYER.STR +
            "/vit : " + PLAYER.VIT +
            "/int : " + PLAYER.INT +
            "/mnd : " + PLAYER.MND +
            "/dex : " + PLAYER.DEX +
            "/agu : " + PLAYER.AGI);

        for (int i = 0; i < count; i++)
        {
            this.transform.GetChild(i).GetComponent<Text>().text = StatusNames[i] + st[i];
        }
    }
}
