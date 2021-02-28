using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(RotationController))]
public class NPC_Ghost : StateMachineBehaviour
{

    private const float jackLight_deathZone = 1.5f;
    private const float jackLight_range = 8f;
    private const float jackLight_angle = 40f;

    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float attackingSpeed = 8f;
    [SerializeField] private float runningAwaySpeed = 12f;
    [SerializeField] private float liveTimeMin = 10f;
    [SerializeField] private float liveTimeMax = 15f;

    [SerializeField] private SoundPlayer soundPlayerDamage;
    [SerializeField] private SoundPlayer soundRandomPiss;

    private RotationController rotationController;

    private Ghost_Chasing chasingState;
    private Ghost_RunningAway runningAwayState;
    private Ghost_Chasing attackingState;
    private Ghost_RunningAway escapingState;

    private float deathTime;

    protected override State initialState => chasingState;

    private void Awake()
    {
        rotationController = GetComponent<RotationController>();
        chasingState = new Ghost_Chasing(this, speed, rotationController, soundPlayerDamage);
        runningAwayState = new Ghost_RunningAway(this, speed, rotationController, false, soundRandomPiss);
        attackingState = new Ghost_Chasing(this, attackingSpeed, rotationController, soundPlayerDamage);
        escapingState = new Ghost_RunningAway(this, runningAwaySpeed, rotationController, true, soundRandomPiss);
    }

    protected override void Start()
    {
        base.Start();
        deathTime = Time.time + Random.Range(liveTimeMin, liveTimeMax);
        soundRandomPiss.PlaySound();
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(chasingState, attackingState, () => LuckDistance() < 3.5f);
        stateMachine.AddTransition(chasingState, runningAwayState, () => IsJackLooking());
        stateMachine.AddTransition(chasingState, escapingState, () => chasingState.Ended);

        stateMachine.AddTransition(attackingState, chasingState, () => LuckDistance() > 4f);
        stateMachine.AddTransition(attackingState, runningAwayState, () => IsJackLooking());
        stateMachine.AddTransition(attackingState, escapingState, () => attackingState.Ended);

        stateMachine.AddTransition(runningAwayState, chasingState, () => !IsJackLooking() && runningAwayState.Ended);

        stateMachine.AddGlobalTransition(escapingState, () => Time.time > deathTime);
        stateMachine.AddGlobalTransition(escapingState, () => Player.Luck.IsDead);
    }

    private float LuckDistance()
    {
        return FlatVector.Distance(Player.Luck.transform.position, transform.position);
    }

    private float JakcDistance()
    {
        return FlatVector.Distance(Player.Jack.transform.position, transform.position);
    }

    private bool IsJackLooking()
    {
        if (JakcDistance() < jackLight_deathZone)
            return false;

        if (JakcDistance() > jackLight_range)
            return false;

        FlatVector targetDirection = transform.position - Player.Jack.transform.position;
        float angle = FlatVector.Angle(targetDirection, Player.Jack.transform.forward);

        return angle < jackLight_angle;
    }

#if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        if (!Player.Instance)
            return;
        if (!Player.Jack)
            return;
        Transform target = Player.Jack.transform;

        Handles.color = Color.red;
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(target.transform.position, target.transform.up, 8f);
        Handles.DrawWireDisc(target.transform.position, target.transform.up, 2.5f);
        Handles.color = Color.green;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            target.transform.position + Quaternion.AngleAxis(-jackLight_angle, Vector3.up) * target.transform.forward * jackLight_deathZone, 
            target.transform.position + Quaternion.AngleAxis(-jackLight_angle, Vector3.up) * target.transform.forward * jackLight_range);
        
        Gizmos.DrawLine(
            target.transform.position + Quaternion.AngleAxis(jackLight_angle, Vector3.up) * target.transform.forward * jackLight_deathZone, 
            target.transform.position + Quaternion.AngleAxis(jackLight_angle, Vector3.up) * target.transform.forward * jackLight_range);
    }

#endif

    private class Ghost_State : State
    {

        protected NPC_Ghost ghost;
        protected float speed;
        protected RotationController rotationController;

        public Ghost_State(NPC_Ghost ghost, float speed, RotationController rotationController)
        {
            this.ghost = ghost;
            this.speed = speed;
            this.rotationController = rotationController;
        }

    }

    private class Ghost_Chasing : Ghost_State
    {

        public bool Ended { get; private set; }
        private SoundPlayer soundPlayer;

        private bool isSprinting;

        public Ghost_Chasing(NPC_Ghost ghost, float speed, RotationController rotationController, SoundPlayer soundPlayer) : base(ghost, speed, rotationController) 
        {
            this.soundPlayer = soundPlayer;
        }

        public override void OnUpdate()
        {
            if (Ended)
                return;

            if (isSprinting && ghost.LuckDistance() < 7.5f)
                isSprinting = false;
            if (!isSprinting && ghost.LuckDistance() > 10f)
                isSprinting = true;

            float currentSpeed = isSprinting ? 20f : speed;
            
            ghost.transform.position = 
                Vector3.MoveTowards(ghost.transform.position, Player.Luck.transform.position, currentSpeed * Time.deltaTime);

            rotationController.LookAt(Player.Luck.transform.position);

            if (ghost.LuckDistance() < 0.25f)
            {
                Player.Luck.ApplyDamage(1, ghost.transform.forward, Team.Bad);
                soundPlayer.PlaySound();
                Ended = true;
            }

        }

    }

    private class Ghost_RunningAway : Ghost_State
    {

        public bool Ended => Time.time > endTime;
        private float endTime;

        private bool destroy;
        private SoundPlayer soundPlayer;

        public Ghost_RunningAway(NPC_Ghost ghost, float speed, RotationController rotationController, bool destroy, SoundPlayer soundPlayer) : base(ghost, speed, rotationController) 
        {
            this.destroy = destroy;
            this.soundPlayer = soundPlayer;
        }

        public override void OnEnter()
        {
            if (destroy)
                Destroy(ghost.gameObject, 15f);
            if (!soundPlayer.IsPlaying)
                soundPlayer.PlaySound();
            endTime = Time.time + Random.Range(0.5f, 4f);
        }

        public override void OnExit()
        {
            if (!soundPlayer.IsPlaying)
                soundPlayer.PlaySound();
        }

        public override void OnUpdate()
        {
            rotationController.LookAt(-(Player.Jack.transform.position - ghost.transform.position).normalized);
            ghost.transform.position += 
                -(Player.Jack.transform.position - ghost.transform.position).normalized * speed * Time.deltaTime;
        }

    }

}