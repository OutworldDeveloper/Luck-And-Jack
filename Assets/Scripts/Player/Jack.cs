using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JackCustomization))]
public class Jack : PlayerCharacter
{

    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private TrailRenderer trailRendererLeft;
    [SerializeField] private TrailRenderer trailRendererRight;
    [SerializeField] private SoundPlayer awakiningSoundPlayer;
    [SerializeField] private SoundPlayer startSleepingSoundPlayer;

    private JackCustomization jackCustomization;

    private JuggernautState juggernautState;
    private JackSleeping prologueSleepingState;
    private JackSleeping lostConnectionState;
    private JackSleeping luckDeadSleepingState;

    private Trigger attackTrigger;

    private bool isPrologueSleeping;

    protected override float speed => 3f;
    protected override Team team => Team.Good;

    public void TryAttack()
    {
        attackTrigger.SetActive();
    }

    public void SetPrologueSleeping(bool b)
    {
        isPrologueSleeping = b;
    }    

    protected override void Awake()
    {
        base.Awake();
        
        jackCustomization = GetComponent<JackCustomization>();

        attackTrigger = stateMachine.CreateTrigger();

        juggernautState = new JuggernautState(this, rotationController, controller, animator, soundPlayer, trailRendererLeft, trailRendererRight);
        prologueSleepingState = new JackSleeping(animator, jackCustomization, awakiningSoundPlayer);
        lostConnectionState = new JackSleepingConnectionLost(animator, jackCustomization, awakiningSoundPlayer, startSleepingSoundPlayer);
        luckDeadSleepingState = new JackSleeping(animator, jackCustomization, awakiningSoundPlayer);
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        stateMachine.AddTransition(movementState, juggernautState, () => attackTrigger.IsActive && juggernautState.CanUse);

        stateMachine.AddTransition(juggernautState, movementState, () => juggernautState.IsEnded);

        stateMachine.AddTransition(movementState, lostConnectionState,
            () => FlatVector.Distance(transform.position, Player.Luck.transform.position) > 14f);

        stateMachine.AddTransition(lostConnectionState, movementState,
            () => FlatVector.Distance(transform.position, Player.Luck.transform.position) < 3.5f);

        stateMachine.AddGlobalTransition(luckDeadSleepingState, () => Player.Luck.IsDead);

        stateMachine.AddGlobalTransition(prologueSleepingState, () => isPrologueSleeping);
        stateMachine.AddTransition(prologueSleepingState, movementState, () => !isPrologueSleeping);
    }

}