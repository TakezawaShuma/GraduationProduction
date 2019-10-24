using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillData
{
    public int[] children = new int[0];
}

public class SkilltreeData
{
    public List<SkillData> skilltree;

    public SkilltreeData()
    {
        skilltree = new List<SkillData>();
    }
}

public class SkilltreeCreater : MonoBehaviour
{
    [SerializeField]
    private GameObject skill;

    [SerializeField]
    private GameObject horizontal;

    [SerializeField]
    private GameObject vertical;

    private List<SkilltreeData> skilltreeDatas;

    private int mostRight = 0;

    const int SIZE = 50;

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
                SkillData skill = new SkillData();
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
        GameObject gameObject = Instantiate(skill, transform);
        gameObject.name = num.ToString();
        gameObject.GetComponent<RectTransform>().localPosition = new Vector2(point.x * SIZE, point.y * -SIZE);

        if (SkillData.children.Length != 0)
        {
            for (int i = 0; i < SkillData.children.Length; i++)
            {
                Vector2 nextPoint = point;

                if(i > 0)
                {
                    nextPoint.x += 1;

                    GameObject horizontalLine = Instantiate(horizontal, transform);
                    horizontalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * SIZE, nextPoint.y * -SIZE);

                    nextPoint.x += 1;

                    mostRight = (int)nextPoint.x;
                }
                else
                {
                    nextPoint.y += 1;

                    GameObject verticalLine = Instantiate(vertical, transform);
                    verticalLine.GetComponent<RectTransform>().localPosition = new Vector2(nextPoint.x * SIZE, nextPoint.y * -SIZE);

                    nextPoint.y += 1;
                }
                Create(skilltreeDatas[0].skilltree[SkillData.children[i]], nextPoint, SkillData.children[i]);
            }
        }
    }
}
