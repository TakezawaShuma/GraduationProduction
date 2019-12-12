//
// MiniIce.cs
//
// Author : Tama
//
// 氷の塊を相手にぶつけるスキル
//

using UnityEngine;

public class MiniIce : SkillBase
{
    [SerializeField]
    private GameObject _skillObject = null;

    private float _startLife;   // 開始ライフ
    private float _life;        // ライフ

    [SerializeField]
    private float _speed = 0.1f;   // 速さ


    private void Start()
    {
        _startLife = 5;
        _life = _startLife;
    }

    private void Update()
    {
        // 移動する
        if (_skillObject.gameObject.activeSelf)
        {
            this.transform.Rotate(0.5f, 0, 0);
            this.transform.position += this.transform.forward * _speed;
        }

        // ライフを更新
        // 死亡判定でオブジェクトを破棄する
        _life -= Time.deltaTime;
        if (IsDead())
        {
            Destroy(this.gameObject);
        }
    }

    private bool IsDead()
    {
        if (_life <= 0)
            return true;
        return false;
    }
}
