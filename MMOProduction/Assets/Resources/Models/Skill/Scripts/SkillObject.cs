//
// SkillObject.cs
//
// Author : Tama
//
// スキル用オブジェクトの基底ファイル
//

using System;
using UnityEngine;


public class SkillObject : MonoBehaviour
{

    // サーバー送信時に必要なデータ
    public int UserID { get; set; }
    public int EnemyID { get; set; }
    public int SkillID { get; set; }

    // 敵にヒット時に呼ばれる関数
    public Action<Collider> HitAction { get; set; }


    protected void StartBase()
    {
        // 関数初期化（空のラムダ式）
        HitAction = ((Collider other) => { });
    }

    // 仮想関数
    public virtual void Play() { }
    public virtual void Stop() { }

    /// <summary>
    /// 他コライダとの当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 敵にヒットした時の処理
        if (other.tag == "Enemy")
        {
            // サーバーにデータを送信する
            WS.WsPlay.Instance.Send(new Packes.Attack(
                0,  // 敵さんのID
                0,  // ユーザーのID
                0,  // 発動したスキルのID
                0   // マップのID
                ).ToJson());
        }

        // 衝突アクションを行う
        HitAction.Invoke(other);
    }
}
