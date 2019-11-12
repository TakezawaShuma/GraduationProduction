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
        // TODO : サーバーにデータを送信する
        HitAction.Invoke(other);
        if (other.tag == "Enemy")
        {
            WS.WsPlay wsp = WS.WsPlay.Instance;
            Enemy enemy = other.GetComponent<Enemy>();
            wsp.Send(new Packes.Attack(enemy.ID, UserRecord.ID, 0, 0).ToJson());
        }
    }
}
