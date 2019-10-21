using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillData
{
    public int[] children = new int[0];
}

public class SkilltreeCreater : MonoBehaviour
{
    [SerializeField]
    private GameObject skill;

    [SerializeField]
    private GameObject horizontal;

    [SerializeField]
    private GameObject vertical;

    private List<SkillData> skillDatas;

    const int SIZE = 50;

    // Start is called before the first frame update
    void Start()
    {
        skillDatas = new List<SkillData>();

        for (int i = 0; i < 10; i++)
        {
            SkillData SkillData = new SkillData();
            skillDatas.Add(SkillData);
        }

        skillDatas[0].children = new int[1] { 1 };
        skillDatas[1].children = new int[2] { 2, 3 };
        skillDatas[2].children = new int[1] { 4 };
        skillDatas[3].children = new int[2] { 5 , 6};
        skillDatas[5].children = new int[1] { 7 };
        skillDatas[6].children = new int[2] { 8, 9 };

        Create(skillDatas[0], new Vector2(1, 1), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create(SkillData SkillData, Vector2 point, int num)
    {
        GameObject gameObject = Instantiate(skill, transform);
        gameObject.name = num.ToString();
        gameObject.GetComponent<RectTransform>().localPosition = new Vector2(point.x * SIZE, point.y * -SIZE);

        if (SkillData.children.Length != 0)
        {
            for (int i = 0; i < SkillData.children.Length; i++)
            {
                Vector2 nextPoint = point;

                if(i % 2 == 0)
                {
                    nextPoint.y += 1;

                    GameObject verticalLine = Instantiate(vertical, transform);
                    verticalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * SIZE, nextPoint.y * -SIZE);

                    nextPoint.y += 1;
                }
                else
                {
                    nextPoint.x += 1;

                    GameObject horizontalLine = Instantiate(horizontal, transform);
                    horizontalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * SIZE, nextPoint.y * -SIZE);

                    nextPoint.x += 1;
                }
                Create(skillDatas[SkillData.children[i]], nextPoint, SkillData.children[i]);
            }
        }
    }
}
