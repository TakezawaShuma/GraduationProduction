using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //プレイヤ情報
    private Player playerCmp;
    [SerializeField]
    private GameObject player = null;

    // 監視用変数
    private int currentHp;
    private int currentMp;
    private int currentMaxHp;
    private int currentMaxMp;

    // UI情報
    [SerializeField]
    private Image job = null;
    [SerializeField]
    private Slider hpRed = null;
    [SerializeField]
    private Slider hpGreen = null;
    [SerializeField]
    private Slider mpYellow = null;
    [SerializeField]
    private Slider mpBlue = null;
    [SerializeField]
    private Image number = null;
    [SerializeField]
    private Image target = null;
    [SerializeField]
    private Text lvName = null;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤ番号判定して+""で取る予定
        //player = GameObject.Find("yuki(Clone)");
        //playerCmp = player.GetComponent<Player>();

        // アイコンやプレイヤ情報の初期設定
        mpYellow.name = "BGMP";
        hpRed.name = "BGHP";
        job.name = "Job";
        number.name = "Num";
        lvName.name = "Player";
        target.name = "Target";
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("player" + UserRecord.ID);
            if (player != null)
            {
                playerCmp = player.GetComponent<Player>();
                playerCmp.MaxHp = 100;
                playerCmp.HP = 30;
                playerCmp.MaxMp = 100;
                playerCmp.MP = 60;
            }
        }
        if (playerCmp)
        {
            StatusUpdate();
        }
    }

    private void StatusUpdate()
    {
        int hp = playerCmp.HP;
        int maxHp = playerCmp.MaxHp;
        int mp = playerCmp.MP;
        int maxMp = playerCmp.MaxMp;

        if (Observar(hp, currentHp) || Observar(maxHp,currentMaxHp))
        {
            // HPの表示を更新する
            hpGreen.value = hp;
        }

        if (Observar(mp, currentMp) || Observar(maxMp,currentMaxMp))
        {
            // MPの表示を更新する
            mpBlue.value = mp;
        }

        currentHp = hp;
        currentMaxHp = maxHp;
        currentMp = mp;
        currentMaxMp = maxMp;
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
}
