using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject damageParent;

    [SerializeField]
    private Sprite[] number = new Sprite[10];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.InputKeyCheckDown(KeyCode.L))
        {
            CreateDamageUI(new Vector3(0, 0, 0), 4);
        }
    }

    void CreateDamageUI(Vector3 pos, int damage)
    {
        var obj = DamageNumber(damage);
        obj.transform.position = WorldtoCamera(pos);

        obj.transform.parent = canvas.transform;
    }

    Vector2 WorldtoCamera(Vector3 pos)
    {
        Vector2 a = Vector2.zero;
        return a;
    }

    GameObject DamageNumber(int num)
    {
        var obj = Instantiate(damageParent);

        int n = num;
        int l = 0;
        while (n != 0)
        {
            int ans = n % 10;
            n /= 10;
            Number(ans).transform.parent = damageParent.transform;
            l++;
        }

        return obj;
    }

    //一桁用
    GameObject Number(int num)
    {
        // 画像生成
        GameObject obj = new GameObject();
        obj.AddComponent<Image>();
        obj.GetComponent<Image>().sprite = number[num];
        return obj;
    }
}
