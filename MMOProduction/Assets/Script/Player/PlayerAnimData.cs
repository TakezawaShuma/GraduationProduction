//
// PlayerAnimData.cs
//

using UnityEngine;

// プレイヤーアニメーションデータ
public class PlayerAnimData
{

    private Animator _animator;
    private GameObject _owner;


    public PlayerAnimData(GameObject owner)
    {
        _animator = owner.GetComponent<Animator>();
    }

    // アニメーションデータ1の時に使用
    public void Move(float speed)
    {
        _animator.SetFloat("speed", speed);
    }

    // アニメーションデータ2の時に使用
    public void Move(bool state)
    {
        _animator.SetBool("walk", state);
    }
}
