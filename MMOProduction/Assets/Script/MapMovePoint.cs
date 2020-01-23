using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMovePoint : MonoBehaviour
{
    [SerializeField, Header("何秒間触れていたら移動するか")]
    private float moveTime = 5f;

    [SerializeField, Header("現在のマップID")]
    private MapID currentMapID = MapID.Base;

    [SerializeField, Header("テキスト")]
    private Text text = null;

    [SerializeField, Header("テキストを使用するか")]
    private bool textUse = false;

    private PlaySceneManager manager = null;

    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("PlaySceneManager").GetComponent<PlaySceneManager>();

        // ユーザーレコードから現在のマップIDを取得
        currentMapID = UserRecord.NextMapId;

        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > moveTime)
        {
            // ここでマップを移動
            manager.SendMoveMap(UserRecord.NextMapId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが移動ポイントに触れたとき
        if (other.tag == "Player" && UserRecord.NextMapId != MapID.Non)
        {
            text.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // プレイヤーが移動ポイントに触れている間
        if(other.tag == "Player")
        {
            currentTime += Time.deltaTime;
            text.text = "クエスト開始まで" + (int)(moveTime - currentTime + 0.5f) +"秒";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーが移動ポイントから離れたとき
        if (other.tag == "Player")
        {
            currentTime = 0;
            text.enabled = false;
        }
    }
}
