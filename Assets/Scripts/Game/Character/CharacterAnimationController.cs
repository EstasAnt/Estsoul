using System;
using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using Character.Shooting;
using Game.Character.Shooting;
using Game.Movement;
using KlimLib.SignalBus;
using UnityEngine;
using UnityDI;

public class CharacterAnimationController : MonoBehaviour {
    
    [Dependency] private SignalBus _SignalBus;
    
    private Animator Animator;
    private MovementController _MovementController;
    private CharacterUnit _CharacterUnit;
    private WeaponController _WeaponController;

    public AnimatorStateInfo CurrentAnimationState => Animator.GetCurrentAnimatorStateInfo(0);
    
    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        Animator = GetComponent<Animator>();
        _MovementController = GetComponentInParent<MovementController>();
        _CharacterUnit = GetComponentInParent<CharacterUnit>();
        _WeaponController = GetComponentInParent<WeaponController>();
    }

    private void Start()
    {
        _SignalBus.Subscribe<AttackAnimationSignal>(OnAttackAnimationSignal, this);
    }

    private void OnAttackAnimationSignal(AttackAnimationSignal signal)
    {
        Animator.SetTrigger(signal.AnimationTriggerName);
    }

    private void Update()
    {
        if (_MovementController == null || Animator == null) return;
        Animator.SetFloat("Horizontal", Mathf.Abs(_MovementController.Horizontal));
        Animator.SetBool("Grounded", _MovementController.IsMainGrounded);
        Animator.SetFloat("DistanseToGround", _MovementController.MinDistanceToGround);
        Animator.SetBool("FallingDown", _MovementController.FallingDown);
        Animator.SetBool("DoubleJump", _MovementController.DoubleJump);
        // Animator.SetBool("WallRun", _MovementController.WallRun);
        // Animator.SetBool("WallSliding", _MovementController.WallSliding);
        // Animator.SetBool("LedgeHang", _MovementController.LedgeHang);
        Animator.SetFloat("Speed", Mathf.Abs(_MovementController.LocalVelocity.x / 50f));
        // Animator.SetBool("Pushing", _MovementController.Pushing);
        // Animator.SetFloat("TimeFallingDown", _MovementController.TimeFallingDown);
        // Animator.SetFloat("TimeNotFallingDown", _MovementController.TimeNotFallingDown);
        // Animator.SetBool("FirstAttack", _WeaponController.MeleeAttacking);
    }

    private void OnDestroy()
    {
        _SignalBus.UnSubscribeFromAll(this);
    }
}