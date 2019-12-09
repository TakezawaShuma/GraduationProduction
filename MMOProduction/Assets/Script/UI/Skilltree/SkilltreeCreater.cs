using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// スキルの基礎データ
/// </summary>
public class SkillData
{
    public SkillData(int Id,int pId,bool act)
    {
        id = Id;
        pearentID = pId;
        active = act;
    }

    public int[] children = new int[0];
    //自身のID
    public int id;
    //親スキルのID
    public int pearentID;
    //子スキルのリスト
    public List<SkillData> childList = new List<SkillData>();
    //自身がアクティブであるか
    public bool active = false;
}

/// <summary>
/// スキルツリーの基礎データ
/// </summary>
public class SkilltreeData
{
    public List<SkillData> skilltree;

    public SkilltreeData()
    {
        skilltree = new List<SkillData>();
    }

    //親IDを見て親の子リストに自身を追加するs
    public void AddChild()
    {
        foreach (var v in skilltree)
        {
            foreach (var vv in skilltree)
            {
                if (v.pearentID == vv.id)
                {
                    //親がないスキルのparentIDは0で
                    if (v.pearentID != 0)
                    {
                        vv.childList.Add(v);
                    }
                    else
                    {
                    }
                }
            }
        }
    }

    public List<SkillData> CheckActiveReturnList(int id_,List<SkillData> sd)
    {
        foreach (var v in skilltree)
        {
            if (v.id == id_)
            {
                if (v.active != true)
                    return sd;
                sd.Add(v);
                if (v.childList.Capacity == 0) break;
                foreach (var vv in v.childList)
                {
                    //sd.Add(vv);
                    sd.AddRange(CheckActiveReturnList(vv.id,sd));
                }
            }
            else
            {
                Debug.Log("存在しないID");
            }
        }
        return sd;
    }

    //public (int[] id,bool[] active) CheckActiveReturnArray(int id_,int[] id,bool[] active)
    //{
    //    foreach(var v in skilltree)
    //    {
    //        if(v.id==id_)
    //        {
    //            Array.Resize(ref id, id.Length + 1);
    //            id[id.Length - 1] = v.id;
    //            Array.Resize(ref active, active.Length + 1);
    //            active[active.Length - 1] = v.active;
                
    //        }
    //        if (v.childList.Capacity == 0) break;
    //        foreach(var vv in v.childList)
    //        {
    //            CheckActiveReturnArray(vv.id, id, active);
    //        }
    //    }

    //    return (id, active);
    //}
}

/// <summary>
/// スキルツリーを作る
/// </summary>
public class SkilltreeCreater : MonoBehaviour
{
    [SerializeField]
    private GameObject skill = null;

    [SerializeField]
    private GameObject horizontal = null;

    [SerializeField]
    private GameObject vertical = null;

    private List<SkilltreeData> skilltreeDatas = null;

    private int mostRight = 0;

    // Start is called before the first frame update
    void Start()
    {
        skilltreeDatas = new List<SkilltreeData>();

        for (int i = 0; i < 10; i++)
        {
            SkilltreeData skilltreeData = new SkilltreeData();
            skilltreeDatas.Add(skilltreeData);

            for (int j = 0; j < 10; j++)
            {
                SkillData skill = new SkillData(0,0,false);
                skilltreeDatas[i].skilltree.Add(skill);
            }
        }

        skilltreeDatas[0].skilltree[0].children = new int[2] { 1, 2 };
        skilltreeDatas[0].skilltree[1].children = new int[1] { 3 };
        skilltreeDatas[0].skilltree[2].children = new int[2] { 4, 5 };
        skilltreeDatas[0].skilltree[3].children = new int[1] { 6 };
        skilltreeDatas[0].skilltree[4].children = new int[1] { 7 };
        skilltreeDatas[0].skilltree[5].children = new int[2] { 8, 9 };

        Create(skilltreeDatas[0].skilltree[0], new Vector2(1, 1), 0);

        skilltreeDatas[1].skilltree[0].children = new int[3] { 1, 2 ,3 };
        skilltreeDatas[1].skilltree[1].children = new int[1] { 4 };
        skilltreeDatas[1].skilltree[2].children = new int[2] { 5, 6 };
        skilltreeDatas[1].skilltree[3].children = new int[1] { 7 };
        skilltreeDatas[1].skilltree[4].children = new int[1] { 8 };
        skilltreeDatas[1].skilltree[5].children = new int[1] { 9 };

        Create(skilltreeDatas[1].skilltree[0], new Vector2(mostRight + 2, 1), 0);

        skilltreeDatas[2].skilltree[0].children = new int[2] { 1, 2 };
        skilltreeDatas[2].skilltree[1].children = new int[1] { 3 };
        skilltreeDatas[2].skilltree[2].children = new int[2] { 4, 5 };
        skilltreeDatas[2].skilltree[3].children = new int[1] { 6 };
        skilltreeDatas[2].skilltree[4].children = new int[1] { 7 };
        skilltreeDatas[2].skilltree[5].children = new int[2] { 8, 9 };

        Create(skilltreeDatas[2].skilltree[0], new Vector2(mostRight + 2, 1), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create(SkillData SkillData, Vector2 point, int num)
    {
        var size = skill.transform.GetComponent<RectTransform>().rect.height;
        GameObject gameObject = Instantiate(skill, transform);
        gameObject.name = num.ToString();
        gameObject.GetComponent<RectTransform>().localPosition = new Vector2(point.x * size, point.y * -size);

        if (SkillData.children.Length != 0)
        {
            for (int i = 0; i < SkillData.children.Length; i++)
            {
                Vector2 nextPoint = point;

                if(i > 0)
                {
                    nextPoint.x += 1;

                    GameObject horizontalLine = Instantiate(horizontal, transform);
                    horizontalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * size, nextPoint.y * -size);

                    nextPoint.x += 1;

                    if (nextPoint.x > mostRight)
                    {
                        mostRight = (int)nextPoint.x;
                    }
                }
                else
                {
                    nextPoint.y += 1;

                    GameObject verticalLine = Instantiate(vertical, transform);
                    verticalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * size, nextPoint.y * -size);

                    nextPoint.y += 1;
                }
                Create(skilltreeDatas[0].skilltree[SkillData.children[i]], nextPoint, SkillData.children[i]);
            }
        }
    }
}
