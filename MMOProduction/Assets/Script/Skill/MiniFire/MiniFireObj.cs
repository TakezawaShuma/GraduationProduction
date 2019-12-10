//
// MiniFireObj.cs
//
// Author : Tama
//
// ミニファイアのスキルオブジェクト
//

using UnityEngine;

public class MiniFireObj : SkillObject
{

    [SerializeField]
    private ParticleSystem _burnEffect = null;


    private void Start()
    {
        _burnEffect.Stop();
        HitAction = BurnAction;
    }

    private void BurnAction(Collider other)
    {
        if (other.tag != "Enemy") return;

        // 燃やすエフェクトを再生
        _burnEffect.Play();

        // 最後に自信を活動停止して終了
        this.gameObject.SetActive(false);
    }
}
