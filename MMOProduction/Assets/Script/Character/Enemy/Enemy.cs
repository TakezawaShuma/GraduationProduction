using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NonPlayer
{

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }

    // 敵が使用できるスキル
    public ScriptableObject enemySkills;

    private UIEnemyHP uIHP = null;

    public UIEnemyHP UI_HP
    {
        get { return uIHP; }
        set { uIHP = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        uIHP.UpdateHP(hp);
        LerpMove();
    }


    // アニメーション関係

    // アニメーションの種類
    enum EnemyAnimetionType
    {
        Attack01,
        Damage,
        Dei,

        Idol,
        Non
    }


    public void PlayAnimetion()
    {

    }

    /// <summary>
    /// ワンプレイ系のアニメーションを再生する
    /// </summary>
    /// <param name="_animetionName">アニメーション遷移名</param>
    public void PlayTriggerAnimetion(string _animetionName)
    {
        gameObject.GetComponent<Animator>().SetTrigger(_animetionName);
        Debug.Log(_animetionName);
    }


    /// <summary>
    /// 攻撃したときに計算する
    /// </summary>
    public void Attacked()
    {
        // 敵が攻撃してきたときに距離を判定する
    }


    /// 
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack 01"
            && collision.gameObject.tag == "Player"){
            Debug.Log("攻撃がヒットしたよ");
        }
    }

    public void DestroyMe()
    {
        Debug.Log("エネミーの名前:" + this.gameObject.name+ "消したよ!");
        Destroy(this.gameObject);
    }
    
}
