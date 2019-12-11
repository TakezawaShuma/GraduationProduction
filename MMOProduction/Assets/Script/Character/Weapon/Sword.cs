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


    private void Start()
    {
        _swordTrail.SetActive(false);        
    }

    /// <summary>
    /// 攻撃演出を再生
    /// </summary>
    public void PlayAttack()
    {
        _swordTrail.SetActive(true);
    }

    /// <summary>
    /// 攻撃演出を停止
    /// </summary>
    public void StopAttack()
    {
        _swordTrail.SetActive(false);
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
