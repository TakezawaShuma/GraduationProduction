//
// SkillPlayer.cs
//
// Author : Tama
//
// スキルを使用するためのスクリプトファイル
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlayer : MonoBehaviour
{

    [SerializeField]
    private SkillObject[] _skillArray = new SkillObject[0];

    public List<SkillObject> SkillList { get; set; }


    private void Start()
    {
        SkillList = new List<SkillObject>();

        foreach (var tmp in _skillArray)
        {
            SkillObject newSkill = tmp;
            newSkill.gameObject.SetActive(false);
            SkillList.Add(newSkill);
        }
    }

    public void Play(int id)
    {
        SkillList[id].gameObject.SetActive(true);
        SkillList[id].Play();
    }

    public void Stop(int id)
    {
        SkillList[id].gameObject.SetActive(false);
        SkillList[id].Stop();
    }

    public SkillObject GetSkill(int id)
    {
        return SkillList[id];
    }
}
