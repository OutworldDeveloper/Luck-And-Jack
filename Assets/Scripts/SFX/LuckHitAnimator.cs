using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LuckHitAnimator : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Luck.Damaged += Luck_Damaged;
    }

    private void OnDestroy()
    {
        Player.Luck.Damaged -= Luck_Damaged;
    }

    private void Luck_Damaged(int damage, int lastHealth, FlatVector direction)
    {
        animator.SetTrigger("Hit");
    }
}