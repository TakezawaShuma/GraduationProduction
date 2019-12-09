using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimatorManager
{
    protected Animator animator;

    //アニメーターのプロパティ
    public Animator ANIMATOR
    {
        get { return animator; }
        set { animator = value; }
    }

    private void AllFalse()
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            animator.SetBool(animator.GetParameter(i).name, false);
        }
    }

    //アニメーションの変更
    public void AnimChange(int animId)
    {
        AllFalse();
        animator.SetBool(animator.GetParameter(animId).name, true);
    }
}
