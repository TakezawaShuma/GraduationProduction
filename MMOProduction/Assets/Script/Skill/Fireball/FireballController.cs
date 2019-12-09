//
// Fireball.cs
//
// 火球のスキルに使用
//

using UnityEngine;


public class FireballController : MonoBehaviour
{

    private float _startLife;   // 開始ライフ
    private float _life;        // ライフ


    private void Start()
    {
        _startLife = 10;
        _life = _startLife;
    }

    private void Update()
    {
        // ライフが尽きたらオブジェクトを破棄する
        _life -= Time.deltaTime;
        if (_life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
