using Character.Shooting;
using Game.Movement;
using UnityEngine;
using UnityDI;

public class CharacterAnimationController : MonoBehaviour {

    private Animator Animator;
    private MovementController _MovementController;
    private CharacterUnit _CharacterUnit;
    private WeaponController _WeaponController;

    private void Awake() {
        ContainerHolder.Container.BuildUp(this);
        Animator = GetComponent<Animator>();
        _MovementController = GetComponentInParent<MovementController>();
        _CharacterUnit = GetComponentInParent<CharacterUnit>();
        _WeaponController = GetComponentInParent<WeaponController>();
    }

    private void Start()
    {
        _WeaponController.MainWeapon.AnimationTriggerEvent += MainWeaponOnAnimationTriggerEvent;
    }

    private void MainWeaponOnAnimationTriggerEvent(string obj)
    {
        Animator.SetTrigger(obj);
    }

    private float SpeedForAnimator => Mathf.Clamp(Mathf.InverseLerp(0.3f, _MovementController.MaxSpeed, Mathf.Abs(_MovementController.Velocity.x)), 0.3f, 1f);
    
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
        Animator.SetFloat("Speed", Mathf.Abs(_MovementController.Velocity.x));
        Animator.SetFloat("SpeedForWalkAnimation", SpeedForAnimator);
        // Animator.SetBool("Pushing", _MovementController.Pushing);
        // Animator.SetFloat("TimeFallingDown", _MovementController.TimeFallingDown);
        // Animator.SetFloat("TimeNotFallingDown", _MovementController.TimeNotFallingDown);
        // Animator.SetBool("FirstAttack", _WeaponController.MeleeAttacking);
    }

    public void SetTrigger(string triggerName)
    {
        Animator.SetTrigger(triggerName);
    }

    public void MainWeaponHit(int attackIndex)
    {
        _WeaponController.MainWeaponHit(attackIndex);
    }

    public void MainWeaponDash(int attackIndex)
    {
        _WeaponController.MainWeaponDash(attackIndex);
    }
    
    public void PlayStepSound()
    {
        _MovementController.PlayMoveSound();
    }
    
    private void OnDestroy()
    {
        if(_WeaponController == null)
            return;
        if(_WeaponController.MainWeapon == null)
            return;
        _WeaponController.MainWeapon.AnimationTriggerEvent -= MainWeaponOnAnimationTriggerEvent;
    }
}