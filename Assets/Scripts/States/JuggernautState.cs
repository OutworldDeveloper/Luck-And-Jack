using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautState : State
{

    public bool IsEnded => Time.time > endTime;
    public bool CanUse => Time.time > cooldownEndTime;

    private PlayerCharacter player;
    private RotationController rotationController;
    private CharacterController controller;
    private Animator animator;
    private SoundPlayer soundPlayer;
    private TrailRenderer trailRendererLeft;
    private TrailRenderer trailRendererRight;

    private float endTime;
    private float cooldownEndTime;

    public JuggernautState(PlayerCharacter player, RotationController rotationController, CharacterController controller, Animator animator, SoundPlayer soundPlayer, TrailRenderer left, TrailRenderer right)
    {
        this.player = player;
        this.rotationController = rotationController;
        this.controller = controller;
        this.animator = animator;
        this.soundPlayer = soundPlayer;
        trailRendererLeft = left;
        trailRendererRight = right;
    }

    public override void OnEnter()
    {
        rotationController.enabled = false;
        endTime = Time.time + 0.3f;
        cooldownEndTime = Time.time + 0.5f;
        //animator.SetBool("Juggernaut", true);
        animator.SetTrigger("Attack");
        CinemachineShake.Shake(2f, 0.3f);
        soundPlayer.PlaySound();
        trailRendererLeft.enabled = true;
        trailRendererRight.enabled = true;
    }

    public override void OnExit()
    {
        rotationController.enabled = true;
        //animator.SetBool("Juggernaut", false);
        //trailRendererLeft.enabled = false;
        //trailRendererRight.enabled = false;
    }

    public override void OnUpdate()
    {
        if (IsEnded)
            return;
        Collider[] hits = Physics.OverlapSphere(player.transform.FlatPosition(), 2.5f);
        foreach (var item in hits)
        {
            if (item.gameObject == player.gameObject)
                continue;

            FlatVector direction = (item.transform.position - player.transform.position).normalized;

            item.GetComponent<IKnockbackReciver>()?.SetKnockback(direction, 12f);
            item.GetComponent<Actor>()?.ApplyDamage(1, direction, player.Team);
        }
    }
}