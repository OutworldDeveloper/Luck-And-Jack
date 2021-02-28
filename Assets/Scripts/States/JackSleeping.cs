using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackSleeping : State
{

    private Animator animator;
    private JackCustomization jackCustomization;
    private SoundPlayer soundPlayer;

    public JackSleeping(Animator animator, JackCustomization jackCustomization, SoundPlayer soundPlayer)
    {
        this.animator = animator;
        this.jackCustomization = jackCustomization;
        this.soundPlayer = soundPlayer;
    }

    public override void OnEnter()
    {
        animator.SetBool("Sleep", true);
        jackCustomization.SetSleeping(true);
    }

    public override void OnExit()
    {
        animator.SetBool("Sleep", false);
        jackCustomization.SetSleeping(false);
        soundPlayer.PlaySound();
    }

}

public class JackSleepingConnectionLost : JackSleeping
{

    private SoundPlayer startSleepingSoundPlayer;

    public JackSleepingConnectionLost(Animator animator, JackCustomization jackCustomization, SoundPlayer soundPlayer, SoundPlayer startSleepingSoundPlayer) : base(animator, jackCustomization, soundPlayer)
    {
        this.startSleepingSoundPlayer = startSleepingSoundPlayer;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        startSleepingSoundPlayer.PlaySound();
        UIController.Instance.ShowHelpText("You are too far from Jack! The connection is lost and he fell asleep.", 4f);
    }

}