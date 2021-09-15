using Core.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Character.Melee;
using Tools.BehaviourTree;
using UnityDI;
using UnityEngine;

namespace Game.Movement.Modules
{
    public class RollModule : MovementModule
    {
        [Dependency]
        private readonly AudioService _AudioService;

        private RollParameters _Parameters;
        
        public event Action<string> AnimationTriggerEvent;
        
        private Animator _characterAnimator;

        private int _startLayer;
        
        public RollModule(RollParameters parameters) : base()
        {
            this._Parameters = parameters;
        }

        public override void Initialize(Blackboard bb)
        {
            base.Initialize(bb);
            _characterAnimator = CommonData.MovementController
                .GetComponentInChildren<Animator>();
            _startLayer = CommonData.BodyCollider.gameObject.layer;
            CommonData.MovementController.AddCantDirectAnimationStateName(_Parameters.AnimationStateName);
            CommonData.MovementController.AddDontMoveAnimationStateName(_Parameters.AnimationStateName);
        }
        
        public void Roll()
        {
            AnimationTriggerEvent?.Invoke(_Parameters.AnimationTriggerName);
        }

        public override void Update()
        {
            if (_characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(_Parameters.AnimationStateName))
            {
                CommonData.BodyCollider.gameObject.layer = LayerMask.NameToLayer(Layers.Names.Corpse);
            }
            else
            {
                CommonData.BodyCollider.gameObject.layer = _startLayer;
            }
        }
    }
    
    [Serializable]
    public class RollParameters
    {
        public string AnimationTriggerName = "Roll";
        public string AnimationStateName = "Roll";
    }
    
}