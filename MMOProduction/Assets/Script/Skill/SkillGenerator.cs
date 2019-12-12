//
// SkillGenerator.cs
//
// スキルの生成を担当するクラス
//

using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{

    // 生成するスキルのオブジェクト配列
    [SerializeField, Header("生成するスキル配列")]
    private SkillBase[] _skillArray = new SkillBase[0];

    // 実際に生成されたスキルのリスト
    private List<GameObject> _skillList;


    private void Start()
    {
        // リストの初期化
        _skillList = new List<GameObject>();
    }

    private void Update()
    {
        // スキルを生成していく
        SkillHandler sh = SkillHandler.Instance;

        for (int i = 0; i < sh.SendDataList.Count; i++)
        {
            SkillHandler.SkillSendData newData = sh.SendDataList[i];
            SkillBase newSkill = Instantiate(_skillArray[(int)newData.id]) as SkillBase;
            // データを適用
            newSkill.Owner = newData.owner;
            newSkill.ID = newData.id;
            newSkill.transform.position = newData.pos;
            newSkill.transform.rotation = newData.rot;
        }

        sh.SendDataList.Clear();
    }
}
