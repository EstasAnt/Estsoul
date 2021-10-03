using System;
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
        public LayerMask LayerMask;
        public string AnimationStateName;
        public float Duration;
        public float Delay;
        public bool StopWhileAttack;
        public bool StopWhileAttackInAir;
        public bool CanDirectWhileAttack = true;
        public bool CanHitRolledTarget;
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

        public virtual void Dash()
        {
            if (CharacterAddForce != Vector2.zero)
            {
                var mc = Weapon?.Owner?.MovementController;
                if (mc != null)
                {
                    var stopAttack = StopWhileAttack;
                    if (!Weapon.Owner.MovementController.IsGrounded && !StopWhileAttackInAir)
                        stopAttack = false;
                    if (stopAttack)
                    {
                        mc.Rigidbody.velocity = new Vector2(0f, mc.Rigidbody.velocity.y);
                    }
                    mc.Rigidbody.AddForce(new Vector2(CharacterAddForce.x * mc.Direction, CharacterAddForce.y)); 
                }
            }
        }
        
        private IEnumerator UseRoutine()
        {
            _HittedDmgbls.Clear();
            if (Delay != 0)
                yield return new WaitForSeconds(Delay);
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
                var hitsCount = Physics2D.OverlapCollider(attackTrigger, new ContactFilter2D { useTriggers = false, layerMask = LayerMask }, hitColliders);
                if(hitsCount <=0)
                    continue;
                foreach (var hit in hitColliders)
                {
                    var dmgbl = hit.GetComponentInParent<IDamageable>();
                    if(dmgbl == null)
                        continue;
                    if(_HittedDmgbls.Contains(dmgbl))
                        continue;
                    if(dmgbl == Weapon.Owner.Damageable)
                        continue;
                    if(dmgbl.OwnerId == Weapon.Owner.Damageable.OwnerId)
                        continue;
                    if(dmgbl.InvulnerableToAttacks)
                        continue;
                    _HittedDmgbls.Add(dmgbl);
                    dmgbl.ApplyDamage(GetDamage(dmgbl));
                }
            }
        }

        private Damage GetDamage(IDamageable dmgbl)
        {
            var instigator = Weapon.Owner.Id;
            var amount = Weapon.Stats.Damage;
            return new Damage(instigator, dmgbl, amount);
        }
    }
}