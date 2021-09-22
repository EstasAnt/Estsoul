using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Movement.RootMotion
{
    public class AnimationMoveRigidbody : MonoBehaviour
    {
        public List<string> AnimationNames;
        
        private Animator _Animator;
        private Rigidbody2D _Rigidbody;

        private void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody2D>();
            _Animator = GetComponentInChildren<Animator>();
        }

        private bool _LastFrameAnimation;
        
        public void Update()
        {
            var isAnim = AnimationNames.Any(_ => _Animator.GetCurrentAnimatorStateInfo(0).IsName(_));
            if(isAnim)
            {
                var pos = _Animator.transform.position;
                _Rigidbody.MovePosition(pos);
                _Animator.transform.localPosition = Vector3.zero;
            }
            else
            {
                if (_LastFrameAnimation)
                {
                    _Animator.transform.localPosition = Vector3.zero;
                }
            }

            _LastFrameAnimation = isAnim;
        }
    }
}