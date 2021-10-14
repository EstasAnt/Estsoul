using Character.Health;
using Character.Shooting;
using Game.Movement;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class CommonData : BlackboardData
    {
        public Transform ObjTransform;
        public Rigidbody2D ObjRigidbody;
        public Collider2D BodyCollider;
        public Collider2D GroundCollider;
        public MovementControllerBase MovementController;
        public IDamageable IDamageable;
        public Animator Animator;
    }
}