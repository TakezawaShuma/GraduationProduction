//
// SkillHandler.cs
//
// スキルを使用するためのハンドルクラス
//

using System.Collections.Generic;
using UnityEngine;

public class SkillHandler
{

    /// <summary>
    /// スキル生成時に使用するデータ
    /// </summary>
    public struct SkillSendData
    {
        public SkillID id;
        public GameObject owner;
        public Vector3 pos;
        public Quaternion rot;

        public SkillSendData(SkillID id, GameObject owner, Vector3 pos, Quaternion rot)
        {
            this.id = id;
            this.owner = owner;
            this.pos = pos;
            this.rot = rot;
        }
    }


    // 生成時のデータを渡す
    private List<SkillSendData> _sendDataList = new List<SkillSendData>();
    public List<SkillSendData> SendDataList { get { return _sendDataList; } }


    /// <summary>
    /// スキルの使用を依頼する
    /// </summary>
    /// <param name="id">使用するスキルのID</param>
    /// <param name="owner">スキルの所有者</param>
    /// <param name="pos">スキルの再生位置</param>
    /// <param name="rot">スキルの回転情報</param>
    /// <returns>要請されたスキルオブジェクトの参照</returns>
    public void RequestToUseSkill(SkillID id, GameObject owner, Vector3 pos = default, Quaternion rot = default)
    {
        SkillSendData data = new SkillSendData(
            id,
            owner,
            pos,
            rot
            );
        _sendDataList.Add(data);
    }


    // シングルトン化 ---------------------------------------------------
    private static SkillHandler _instance;
    public static SkillHandler Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SkillHandler();
            return _instance;
        }
    }
    private SkillHandler() { }
}
