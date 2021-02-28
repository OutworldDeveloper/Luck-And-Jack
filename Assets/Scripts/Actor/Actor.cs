using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void OnDamaged(int damage, int lastHealth, FlatVector direction);
public delegate void OnHealed(int healAmount, int lastHealth);

public abstract class Actor : StateMachineBehaviour
{

    [SerializeField] private float eyesOffset = 1.75f;
    [SerializeField] private int initialHealth = 1;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private UnityEvent onDeathEvent;

    public event OnDamaged Damaged;
    public event OnHealed Healed;
    public event Action Died;

    public Vector3 EyesPosition => transform.position + Vector3.up * eyesOffset;
    public FlatVector LookDirection { get; private set; }
    public int Health => health;
    public int MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
    public Team Team => team;

    private int health;
    protected abstract Team team { get; }

    public void SetLookDirection(FlatVector direction)
    {
        LookDirection = direction;
    }

    public void SetLookDirection(Vector3 direction)
    {
        LookDirection = new FlatVector(direction);
    }

    public void ApplyDamage(int damage, FlatVector direction, Team team)
    {
        if (IsDead) 
            return;
        if (this.team == team)
            return;
        int lastHealth = health;
        health = Mathf.Max(0, health - damage);
        Damaged?.Invoke(damage, lastHealth, direction);
        if (health <= 0)
        {
            IsDead = true;
            OnDeath();
            Died?.Invoke();
            onDeathEvent.Invoke();
        }     
    }

    public void ApplyHeal(int healAmount)
    {
        if (IsDead)
            return;
        int lastHealth = health;
        health = Mathf.Min(maxHealth, health + healAmount);
        Healed?.Invoke(healAmount, lastHealth);
    }

    protected virtual void Awake()
    {
        health = initialHealth;
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddGlobalTransition(GetDeathState(), () => IsDead);
    }

    protected virtual void OnDeath() { }

    protected virtual State GetDeathState()
    {
        return new DeathState();
    }

    private class DeathState : State { }

#if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (LookDirection.Vector3 != Vector3.zero)
            Gizmos.DrawRay(transform.position, LookDirection.Vector3);
        Gizmos.DrawRay(EyesPosition, transform.forward);
    }

#endif

}