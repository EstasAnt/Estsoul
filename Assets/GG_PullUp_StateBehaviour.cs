using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Movement;
using Tools.Unity;
using UnityDI;
using UnityEngine;

public class GG_PullUp_StateBehaviour : StateMachineBehaviour
{
    public AnimationCurve MoveCurveX;
    public AnimationCurve MoveCurveY;
    // public float MoveCurveMultiplier;

    private UnityEventProvider _EventProvider;
    private Rigidbody2D _rigidbody;
    private MovementController _movementController;
    
    private Vector2 _StartPos;
    private float _PullUpTimer;
    private float _StartGravityScale;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_EventProvider == null)
            _EventProvider = ContainerHolder.Container.Resolve<UnityEventProvider>();
        if (_rigidbody == null)
            _rigidbody = animator.GetComponentInParent<Rigidbody2D>();
        if (_movementController == null)
            _movementController = animator.GetComponentInParent<MovementController>();
        _StartPos = _rigidbody.position;
        _StartGravityScale = _rigidbody.gravityScale;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var normTime = stateInfo.normalizedTime;
        var deltaPosX = MoveCurveX.Evaluate(normTime) * _movementController.Direction;
        var deltaPosY = MoveCurveY.Evaluate(normTime);
        var newPos = new Vector2(deltaPosX, deltaPosY) + _StartPos;
        _rigidbody.MovePosition(newPos);
        _rigidbody.gravityScale = 0;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody.gravityScale = _StartGravityScale;
    }
}
