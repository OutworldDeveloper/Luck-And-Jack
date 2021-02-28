using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LuckDeathAnimator : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Player.Luck.Died += Luck_Died;
    }

    private void OnDestroy()
    {
        Player.Luck.Died -= Luck_Died;
    }

    private void Luck_Died()
    {
        animator.SetTrigger("Death");
    }

}