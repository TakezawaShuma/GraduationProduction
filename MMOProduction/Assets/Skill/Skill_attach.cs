using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_attach : MonoBehaviour
{
    //スキルの最初の座標
    private Vector3 startPosition;

    private RectTransform rectT;

    private Vector3 mouse;
    private Vector3 mouse3d;

    //スキルを追従する変数
    bool isTrack = false;

    //スキルを戻す変数
    bool isBack = true;

    //スキル枠の中心座標
    Vector3 slot_c;


    // Use this for initialization
    void Start()
    {
        //スキルの座敷所得
        startPosition = transform.position;

        rectT = this.gameObject.GetComponent<RectTransform>();
        //スキル枠の中心を所得
        slot_c = (GameObject.Find("slot")).GetComponent<Renderer>().bounds.center;
    }


    // Update is called once per frame
    void Update()
    {
        float widthd = (this.rectT.sizeDelta.x / 2) * this.rectT.lossyScale.x;
        float heightd = (this.rectT.sizeDelta.x / 2) * this.rectT.lossyScale.y;
        //スキルの座標移動
        if (Input.GetMouseButtonDown(0))
        {
            if ((this.transform.position.x - widthd <= (Input.mousePosition.x)) && (this.transform.position.x+widthd>=(Input.mousePosition.x)))
            {
                if (this.transform.position.y - heightd <= (Input.mousePosition.y) && (this.transform.position.y + heightd >= (Input.mousePosition.y)))
                {
                    isTrack = true;
                    isBack = true;
                }
            }
        }
        //Trackがtrueならスキルをマウスに追従する
        if (isTrack == true)
        {
            mouse = Input.mousePosition;
            mouse3d = Camera.main.ScreenToWorldPoint(mouse);
            transform.position = mouse;
        }
        //マウスの左ボタンを離したら
        if(Input.GetMouseButtonUp(0))
        {
            isTrack = false;
            //スキルを元の位置に戻す
            if (isBack == true)
            {
                transform.position = startPosition;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        //スキルをスキル枠に付ける
        OnCollisionTag(other.gameObject);
        if (Input.GetMouseButtonUp(1))
        {
            transform.position = startPosition;
        }
    }

    void OnCollisionTag(GameObject slot)
    {
        //マウスの左ボタンを離したときSlotタグが重なったらisBackをfalseにする
        if (Input.GetMouseButtonUp(0))
        {
            if (slot.tag == "Slot")
            {
                isBack = false;
                isTrack = false;
                transform.position=slot.transform.position;
            }
        }
    }
}
