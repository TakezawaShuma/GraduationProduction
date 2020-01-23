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

    [SerializeField]
    private QuestMapMoveImage questMapMoveImage = null;

    private PlaySceneManager manager = null;

    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        questMapMoveImage.SetState(false);

        // ユーザーレコードから現在のマップIDを取得
        currentMapID = UserRecord.MapID;

        slider.maxValue = moveTime;
        slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > moveTime)
        {
            manager = GameObject.Find("PlaySceneManager").GetComponent<PlaySceneManager>();
            // ここでマップを移動
            manager.SendMoveMap(UserRecord.MapID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentMapID != UserRecord.MapID)
        {
            // プレイヤーが移動ポイントに触れたとき
            if (other.tag == "Player" && UserRecord.MapID != MapID.Non)
            {
                slider.gameObject.SetActive(true);
                questMapMoveImage.SetState(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentMapID != UserRecord.MapID)
        {
            // プレイヤーが移動ポイントに触れている間
            if (other.tag == "Player" && UserRecord.MapID != MapID.Non)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // プレイヤーが移動ポイントから離れたとき
        if (other.tag == "Player")
        {
            currentTime = 0;
            slider.gameObject.SetActive(false);
            questMapMoveImage.SetState(false);
        }
    }
}
