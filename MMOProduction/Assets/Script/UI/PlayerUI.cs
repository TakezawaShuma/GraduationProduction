using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// プレイヤーの情報をUI化する
/// </summary>
public class PlayerUI : MonoBehaviour
{
    //プレイヤ情報
    private Player playerCmp;

    public Player PLAYER_CMP
    {
        set { playerCmp = value; }
    }

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
    private Text lv = null;
    [SerializeField]
    private Text playerName = null;

    public string PLAYER_NAME
    {
        set { playerName.text = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // アイコンやプレイヤ情報の初期設定
        mpYellow.name = "BGMP";
        hpRed.name = "BGHP";
        job.name = "Job";
        number.name = "Num";
        lv.name = "LV";
        playerName.name = "Player";
        target.name = "Target";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCmp)
        {
            StatusUpdate();
        }
    }

    private void StatusUpdate()
    {
        int hp = playerCmp.hp;
        int maxHp = playerCmp.maxHp;
        int mp = playerCmp.mp;
        int maxMp = playerCmp.maxMp;

        if (Observar(hp, currentHp) || Observar(maxHp, currentMaxHp))
        {
            // HPの表示を更新する
            hpGreen.value = hp;
            hpGreen.maxValue = maxHp;
            Debug.Log("MaxHP : "+maxHp);
        }

        if (Observar(mp, currentMp) || Observar(maxMp,currentMaxMp))
        {
            // MPの表示を更新する
            mpBlue.value = mp;
            mpBlue.maxValue = maxMp;
            Debug.Log("MaxMP : " + maxMp);
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
