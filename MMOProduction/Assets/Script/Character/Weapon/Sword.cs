//
// Sword.cs
//
// 剣のオブジェクト
//

using UnityEngine;

public class Sword : WeaponBase
{

    [SerializeField]
    private GameObject _swordTrail = null;

    [SerializeField]
    private Transform _startPoint = null;
    [SerializeField]
    private Transform _endPoint = null;

    [SerializeField]
    private Transform _owner = null;

    private SwordTrailCreator _effect;


    private void Start()
    {
        // 剣の軌跡を作成
        SwordTrailCreator newTrail = Instantiate(_swordTrail).GetComponent<SwordTrailCreator>();
        newTrail.SetPoint(_startPoint, _endPoint);
        newTrail.Owner = this.transform;
        _effect = newTrail;
        newTrail.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_effect == null)
        {
            Start();
        }
    }

    /// <summary>
    /// 攻撃演出を再生
    /// </summary>
    public void PlayAttack()
    {
        _effect.On();
    }

    /// <summary>
    /// 攻撃演出を停止
    /// </summary>
    public void StopAttack()
    {
        _effect.Off();
    }

    /// <summary>
    /// オブジェクトのアクティブ状態を変更
    /// </summary>
    /// <param name="activeFlag">アクティブ状態フラグ</param>
    public void SetActive(bool activeFlag)
    {
        this.gameObject.SetActive(activeFlag);
    }
}
