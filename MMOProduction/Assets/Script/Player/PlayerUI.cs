using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //プレイヤ情報
    private Player playerCmp;
    [SerializeField]
    private GameObject player;

    // 監視用変数
    private int currentHp;
    private int currentMp;

    // UI情報
    [SerializeField]
    private Image job;
    [SerializeField]
    private Slider hpRed;
    [SerializeField]
    private Slider hpGreen;
    [SerializeField]
    private Slider mpYellow;
    [SerializeField]
    private Slider mpBlue;
    [SerializeField]
    private Image number;
    [SerializeField]
    private Image target;
    [SerializeField]
    private Text lvName;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤ番号判定して+""で取る予定
        player = GameObject.Find("yuki(Clone)");
        playerCmp = player.GetComponent<Player>();

        // アイコンやプレイヤ情報の初期設定
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("player" + Retention.ID);
            playerCmp = player.GetComponent<Player>();
        }

        StatusUpdate();
    }

    private void StatusUpdate()
    {
        int hp = playerCmp.maxHp / playerCmp.hp;
        int mp = playerCmp.maxMp / playerCmp.mp;

        if (Observar(hp, currentHp))
        {
            // HPの表示を更新する
            hpGreen.value = hp;
        }

        if (Observar(mp, currentMp))
        {
            // MPの表示を更新する
            mpBlue.value = mp;
        }

        currentHp = hp;
        currentMp = mp;
    }

    //ステータスに変化があるか調べる
    private bool Observar(int data,int current)
    {
        if(current != data)
        {
            return true;
        }
        return false;
    }

    private void HPUpdate()
    {

    }
}
