// 
// MiniIceObj.cs
//
// スキル用オブジェクト
//

using UnityEngine;

public class MiniIceObj : SkillObject
{

    [SerializeField]
    private ParticleSystem _freezeEffect = null;


    private void Start()
    {
        _freezeEffect.Stop();
        HitAction = FreezeAction;
    }

    private void FreezeAction(Collider other)
    {
        if (other.tag != "Enemy") return;

        // 燃やすエフェクトを再生
        _freezeEffect.Play();

        // 最後に自信を活動停止して終了
        this.gameObject.SetActive(false);
    }
}
