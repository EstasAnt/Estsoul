using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

namespace Character.Shooting {
    public abstract class LongRangeWeapon<P, D> : Weapon where P : Projectile<D> where D : ProjectileDataBase, new() {
        public override ItemType ItemType => ItemType.Weapon;
        public string ProjectileName;
        public string ShotCameraShakePresetName;

        protected float RandomDispersionAngle => Random.Range(-_Stats.DispersionAngle / 2, _Stats.DispersionAngle / 2);

        public virtual P GetProjectile() {
            //return Instantiate(ProjectilePrefab);
            return VisualEffect.GetEffect<P>(ProjectileName);
        }

        public virtual D GetProjectileData() {
            var data = new D {
                Damage = GetDamage()
            };
            return data;
        }

        protected virtual void FireProjectile()
        {
            if (ProCamera2DShake.Instance != null && !string.IsNullOrEmpty(ShotCameraShakePresetName))
                ProCamera2DShake.Instance.Shake(ShotCameraShakePresetName);
            for (int i = 0; i < Stats.ProjectilesInShot; i++) {
                var projectile = GetProjectile();
                var data = GetProjectileData();
                projectile.Setup(data);
                projectile.Play();
                if(Owner.MovementController != null && Owner.MovementController.CanMove)
                    AddRecoil(data.Rotation * -Vector3.forward);
            }
        }
        
        public override void PerformShot() {
            base.PerformShot();
        }

        public override void Hit(int attackIndex)
        {
            FireProjectile();
        }

        private void AddRecoil(Vector2 direction) {
            direction.y *= 0.6f;
            Owner.MovementController.Rigidbody.AddForce(direction * Stats.RecoilForce);
        }
    }
}