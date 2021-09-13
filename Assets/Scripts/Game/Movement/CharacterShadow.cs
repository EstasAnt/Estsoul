using System;
using UnityEngine;

namespace Game.Movement
{
    public class CharacterShadow : MonoBehaviour
    {
        public SpriteRenderer _ShadowSprite;
        
        private MovementController _MovementController;
        
        private void Awake()
        {
            _MovementController = GetComponentInParent<MovementController>();
        }

        private void Update()
        {
            _ShadowSprite.enabled = _MovementController.IsMainGrounded;
        }
    }
}