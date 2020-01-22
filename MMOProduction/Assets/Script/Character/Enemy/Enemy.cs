using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NonPlayer
{

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    
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
    }

    public void PlayAttackAnimation(int _skillId)
    {
        // アニメーションをスキル事に再生させる
        PlayTriggerAnimetion(skillTable.FindSkill(_skillId).animation);
    }



    /// <summary>
    /// 攻撃したときに計算する
    /// </summary>
    public bool Attacked(GameObject _player, int _skillId)
    {
        // 敵が攻撃したときに距離を判定する
        Vector3 vec = this.transform.position - _player.transform.position;
        float distance = vec.magnitude;
        skill_table.skill_data useSkill = skillTable.FindSkill(_skillId);

        if (useSkill.range > distance)
        {
            // 敵の攻撃が当たった時の処理をする
            // 攻撃が当たり、ダメージが発生したらダメージを返す
            return true;
        }
        // ダメージが発生しなかったら-1を返す
        return false;
    }


    public void DestroyMe()
    {
        Debug.Log("エネミーの名前:" + this.gameObject.name + "消したよ!");
        Destroy(this.gameObject);
    }

}
