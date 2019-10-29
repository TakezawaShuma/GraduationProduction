using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// テスト用
// 火炎球の処理
public class FireBall : MonoBehaviour
{
    float _count;

    // Start is called before the first frame update
    void Start()
    {
        _count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _count += Time.deltaTime;
        if(_count >= 5)
        {
            Destroy(this.gameObject);
        }
    }
}
