using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{

    [SerializeField]
    private AttackCollider[] _attackObjs = new AttackCollider[1];
    //private List<AttackCollider> _attackColliders = new List<AttackCollider>();

    private WS.WsPlay _wsp;


    private void Update()
    {
        AttackNotify notify = AttackNotify.Instance;
        if (notify.NotifyDataList.Count <= 0) return;

        // 攻撃オブジェクトを生成する
        for(int i = 0; i < notify.NotifyDataList.Count; i++)
        {
            AttackCollider obj = Instantiate(_attackObjs[0]);
            //obj.transform.position = notify.NotifyDataList[i].spawn_pos;
            int user_id = notify.NotifyDataList[i].user_id;
            int skill_id = notify.NotifyDataList[i].skill_id;
            obj.SetSendData(_wsp, user_id, skill_id);
        }

        // データをリセット
        notify.NotifyDataList.Clear();
    }

    public void SetWsp(WS.WsPlay wsp)
    {
        _wsp = wsp;
    }
}
