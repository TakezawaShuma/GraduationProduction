//
// AttackCollider.cs
//
// 攻撃用当たり判定
//

using System;
using UnityEngine;


public class AttackCollider : MonoBehaviour
{

    //[SerializeField]
    //private Mesh _attackMesh = null;

    private WS.WsPlay _wsp;
    private int _enemyID;
    public int EnemyID { get { return _enemyID; } set { _enemyID = value; } }
    private int _userID;
    public int UserID { get { return _userID; } set { _userID = value; } }
    private int _skillID;
    public int SkillID { get { return _skillID; } set { _skillID = value; } }

    // 攻撃時のアクション
    private Action _attackAction;
    public Action AttackAction { get { return _attackAction; } set { _attackAction = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // サーバーにデータを送信する
            //int enemyID = other.GetComponent<Enemy>().ID;
            int enemyID = 0;
            _wsp.Send(new Packes.Attack(enemyID, _userID, _skillID, 0).ToJson());

            // デバッグ表示
            Debug.Log("Attack successfull : (" + enemyID + ", " +  _userID  + ", " + _skillID + ")");
        }
    }

    public void SetSendData(WS.WsPlay wsp, int user_id, int skill_id)
    {
        _wsp = wsp;
        _userID = user_id;
        _skillID = skill_id;
        //Debug.Log(_wsp.ToString());
    }
}
