using System.Collections;
using System.Collections.Generic;
using Game.Movement;
using UnityEngine;

public class GG_Roll_StateBehaviour : StateMachineBehaviour
{
    public AnimationCurve Curve;
    public float Distance;
    public float GravityCof;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private MovementController _movementController;

    private Vector2 _startPosition;
    private float _startGravityScale;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_rigidbody == null)
            _rigidbody = animator.GetComponent<Rigidbody2D>();
        if (_movementController == null)
            _movementController = animator.GetComponent<MovementController>();
        _startPosition = _rigidbody.position;
        _startGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var currentPos = _rigidbody.position;
        
        var newXPos = _startPosition.x + Distance * Curve.Evaluate(stateInfo.normalizedTime) * _movementController.Direction;
        //var nextYPos = currentPos.y + Physics2D.gravity.y * _rigidbody.gravityScale;
        var gravityAdd = Physics2D.gravity * GravityCof * Time.deltaTime;
        
        _rigidbody.MovePosition(new Vector2(newXPos, currentPos.y) + gravityAdd);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody.gravityScale = _startGravityScale;
    }
}
