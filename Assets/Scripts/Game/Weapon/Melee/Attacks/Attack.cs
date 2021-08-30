﻿using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using Character.Shooting;
using UnityEngine;

namespace Game.Character.Melee
{
    public class Attack : MonoBehaviour
    {
        public Weapon Weapon { get; private set; }
        
        public List<Collider2D> AttackTriggers;
        public string AnimationStateName;
        public float Duration;
        public float Delay;
        public bool StopWhileAttack;
        public Vector2 CharacterAddForce;

        private List<IDamageable> _HittedDmgbls = new List<IDamageable>();

        protected void Awake()
        {
            Weapon = GetComponentInParent<Weapon>();
            GetComponentsInChildren(AttackTriggers);
            AttackTriggers.ForEach(_ => _.enabled = false);
        }

        public virtual void Use()
        {
            StopAllCoroutines();
            StartCoroutine(UseRoutine());
        }

        private IEnumerator UseRoutine()
        {
            _HittedDmgbls.Clear();
            if (Delay != 0)
                yield return new WaitForSeconds(Delay);

            if (CharacterAddForce != Vector2.zero)
            {
                var mc = Weapon?.PickableItem?.Owner?.MovementController;
                if (mc != null)
                {
                    if (StopWhileAttack)
                    {
                        mc.Rigidbody.velocity = new Vector2(0f, mc.Rigidbody.velocity.y);
                    }
                    mc.Rigidbody.AddForce(CharacterAddForce * mc.Direction);    
                }
            }
            AttackTriggers.ForEach(_ => _.enabled = true);
            yield return new WaitForSeconds(Duration);
            AttackTriggers.ForEach(_ => _.enabled = false);
            _HittedDmgbls.Clear();
        }

        private void FixedUpdate()
        {
            foreach (var attackTrigger in AttackTriggers)
            {
                if(!attackTrigger.enabled)
                    continue;
                var hitColliders = new List<Collider2D>();
                var hitsCount = Physics2D.OverlapCollider(attackTrigger, new ContactFilter2D(){ useTriggers = true }, hitColliders);
                if(hitsCount <=0)
                    continue;
                foreach (var hit in hitColliders)
                {
                    var dmgbl = hit.GetComponentInParent<IDamageable>();
                    if(dmgbl == null)
                        continue;
                    if(_HittedDmgbls.Contains(dmgbl))
                        continue;
                    _HittedDmgbls.Add(dmgbl);
                    dmgbl.ApplyDamage(GetDamage(dmgbl));
                }
            }
        }

        private Damage GetDamage(IDamageable dmgbl)
        {
            var instigator = Weapon.PickableItem.Owner.OwnerId;
            var amount = Weapon.Stats.Damage;
            return new Damage(instigator, dmgbl, amount);
        }
    }
}