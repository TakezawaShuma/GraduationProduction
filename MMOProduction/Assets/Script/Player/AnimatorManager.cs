using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager
{
    private Animator animator;

    public Animator ANIMATOR
    {
        get { return animator; }
    }

    public AnimatorManager(Animator animator)
    {
        this.animator = animator;
    }

    public void AllFalse()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
    }

    public void Idle()
    {
        AllFalse();
        animator.SetBool("Idle", true);
    }

    public void Walk()
    {    
       AllFalse();
       animator.SetBool("Walk", true);
    }

    public void Run()
    {
        AllFalse();
        animator.SetBool("Run", true);
    }

    public void NormalAttack()
    {
        AllFalse();
        animator.SetBool("Attack", true);
    }
}
