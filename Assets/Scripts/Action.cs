using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        animator.SetFloat("speed", 0);
        animator.SetBool("isAttacking", false);
        //animator.SetBool("isJumping", false);
        //animator.SetBool("isLanding", false);
    }

    public void Move(float speed)
    {
        animator.SetBool("isAttacking", false);
        animator.SetFloat("speed", speed);
    }

    public void Attack()
    {

        animator.SetBool("isAttacking", true);
    }

    public void Die()
    {
        animator.SetTrigger("Death");
    }

    public void Jump(bool isjumping)
    {
        animator.SetBool("isJumping", isjumping);
    }
    public void Land(bool islanding)
    {
        animator.SetBool("isLanding", islanding);
    }
}
