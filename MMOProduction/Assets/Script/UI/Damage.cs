using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Sprite[] number = new Sprite[10];

    public void CreateDamageUI(Vector3 pos, int damage)
    {
        var obj = DamageNumber(damage);
        obj.transform.parent = canvas.transform;

        obj.transform.localPosition = WorldtoCamera(pos);
    }

    private Vector2 WorldtoCamera(Vector3 pos)
    {
        //var ret = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        Vector2 ret = new Vector2(Random.Range(-50,50), Random.Range(-50, 50));
        return ret;
    }

    private GameObject DamageNumber(int num)
    {
        GameObject obj = new GameObject();
        obj.AddComponent<DamageMove>();
        int n = num;
        int l = 0;
        while (n != 0)
        {
            int ans = n % 10;
            n /= 10;
            Number(ans).transform.parent = obj.transform;
            l++;
        }

        for (int i = 0;i < obj.transform.childCount;i++)
        {
            obj.transform.GetChild(i).transform.position = new Vector3(55 * -i, 0, 0);
        }

        return obj;
    }

    //一桁用
    private GameObject Number(int num)
    {
        // 画像生成
        GameObject obj = new GameObject();
        Image image = obj.AddComponent<Image>();
        image.raycastTarget = false;
        image.sprite = number[num];
        return obj;
    }
}
