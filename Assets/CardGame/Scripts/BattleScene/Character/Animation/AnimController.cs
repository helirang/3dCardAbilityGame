using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(AnimationPlayer))]
public class AnimController : MonoBehaviour
{
    [SerializeField] AnimationPlayer animPlayer;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (animator == null) animator = this.GetComponent<Animator>();
        if (animPlayer == null) animPlayer = this.GetComponent<AnimationPlayer>();
    }

    public void Attack(
    System.Action beginCallback = null,
    System.Action midCallback = null,
    System.Action endCallback = null)
    {
        animPlayer.Play("Attack", beginCallback, midCallback, endCallback);
    }

    public void StartMove()
    {
        animator.SetBool("isMove", true);
    }

    public void StopMove()
    {
        animator.SetBool("isMove", false);
    }

    public void StopAnim()
    {
        animator.SetBool("isMove", false);
        animator.SetTrigger("StopUpper");
    }

    public void Hit(
    System.Action beginCallback = null,
    System.Action midCallback = null,
    System.Action endCallback = null)
    {
        animPlayer.Play("Hit", beginCallback, midCallback, endCallback);
    }

    public void Dead()
    {
        animator.SetTrigger("StopUpper");
        animator.SetTrigger("Died");
    }
}
