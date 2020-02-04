using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NonPlayer
{

    protected EnemyAnim.PARAMETER_ID lastAnimation;
    public EnemyAnim.PARAMETER_ID enemyAnimType = EnemyAnim.PARAMETER_ID.IDLE;
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
        am = new AnimatorManager();
        am.ANIMATOR = animator_;
        lastPos = transform.position;
        lastDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        uIHP.UpdateHP(hp);
        LerpMove();
        Animation();
    }

    /// <summary>
    /// 位置情報を更新する
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="_dir"></param>
    public override void UpdatePostionData(float _x, float _y, float _z, float _dir)
    {
        // 向きを決める
        lastDir = transform.rotation;
        nextDir = Quaternion.Euler(0, _dir, 0);

        // 位置を決める
        lastPos = transform.position;
        nextPos = new Vector3(_x, CheckSurface(_y), _z);
        if (lastPos == nextPos)
        {
            enemyAnimType = EnemyAnim.PARAMETER_ID.IDLE;
        }
        else { enemyAnimType = EnemyAnim.PARAMETER_ID.WALK; }

        // カウントを初期化
        nowFlame = 0;
    }

    // アニメーション関係



    /// <summary>
    /// アニメーションの再生
    /// </summary>
    private void Animation()
    {
        if (lastAnimation != enemyAnimType)
        {
            am.AnimChange((int)enemyAnimType);
            lastAnimation = enemyAnimType;
        }
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

    public void ResetAnimation()
    {
        enemyAnimType = EnemyAnim.PARAMETER_ID.IDLE;
    }

    public void DestroyMe()
    {
        Debug.Log("エネミーの名前:" + this.gameObject.name + "消したよ!");
        Destroy(this.gameObject);
    }

}
