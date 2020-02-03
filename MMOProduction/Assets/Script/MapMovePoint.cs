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

    [SerializeField, Header("スライダー")]
    private Slider slider = null;

    private PlaySceneManager manager = null;

    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // ユーザーレコードから現在のマップIDを取得
        currentMapID = UserRecord.NextMapId;

        slider.maxValue = moveTime;

        slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime >= moveTime)
        {
            // ここでマップを移動
            manager = GameObject.Find("PlaySceneManager").GetComponent<PlaySceneManager>();

            manager.SendMoveMap(UserRecord.NextMapId);

            currentTime = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが移動ポイントに触れたとき
        if (other.tag == "Player" && UserRecord.NextMapId != MapID.Non)
        {
            slider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // プレイヤーが移動ポイントに触れている間
        if(other.tag == "Player")
        {
            if (UserRecord.NextMapId != MapID.Non)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime;
                if(currentTime >= moveTime)
                {
                    slider.value = moveTime;
                }
            }
            else
            {
                currentTime = 0;
                slider.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーが移動ポイントから離れたとき
        if (other.tag == "Player" && UserRecord.NextMapId != MapID.Non)
        {
            currentTime = 0;
            slider.gameObject.SetActive(false);
        }
    }
}
