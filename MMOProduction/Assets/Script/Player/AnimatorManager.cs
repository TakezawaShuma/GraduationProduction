using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager
{
    private Animator animator;

    public AnimatorManager(Animator animator)
    {
        this.animator = animator;
    }

    private void AllFalse()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
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

    public void Speed(float speed)
    {
        animator.SetFloat("speed", speed);
    }

    public void Kill()
    {
        //AllFalse();
        animator.SetTrigger("kill");
    }
}
