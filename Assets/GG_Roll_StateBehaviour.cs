using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Movement;
using Tools.Unity;
using UnityDI;
using UnityEngine;

public class GG_Roll_StateBehaviour : StateMachineBehaviour
{
    public Vector2 Force;
    public float Acceleration;
 
    private UnityEventProvider _EventProvider;
    private Rigidbody2D _rigidbody;
    private MovementController _movementController;

    private float _StartGroundAcceleration;
    private float _StartAirAcceleration;

    private Coroutine _MoveRoutine;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_EventProvider == null)
            _EventProvider = ContainerHolder.Container.Resolve<UnityEventProvider>();
        if (_rigidbody == null)
            _rigidbody = animator.GetComponentInParent<Rigidbody2D>();
        if (_movementController == null)
            _movementController = animator.GetComponentInParent<MovementController>();
        if(_MoveRoutine != null)
            _EventProvider.StopCoroutine(_MoveRoutine);
        _MoveRoutine = _EventProvider.StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return null;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _rigidbody.AddForce(new Vector2(Force.x * _movementController.Direction, Force.y));
        _StartGroundAcceleration = _movementController.WalkParameters.GroundAcceleration;
        _StartAirAcceleration = _movementController.WalkParameters.AirAcceleration;
        _movementController.WalkParameters.GroundAcceleration = Acceleration;
        _movementController.WalkParameters.AirAcceleration = Acceleration; 
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _movementController.WalkParameters.GroundAcceleration = _StartGroundAcceleration;
        _movementController.WalkParameters.AirAcceleration = _StartAirAcceleration;
    }
}
