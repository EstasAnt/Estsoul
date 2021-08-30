using RC.UI.Markers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Shooting {
    [RequireComponent(typeof(Weapon))]
    public class FallDownIfNoAmmo : MonoBehaviour {

        private Weapon _Weapon;

        private void Awake() {
            _Weapon = GetComponent<Weapon>();
            _Weapon.OnNoAmmo += OnNoAmmo;
        }

        private void OnNoAmmo() {
            StartCoroutine(MakeFallingDownRoutine());
        }

        private IEnumerator MakeFallingDownRoutine() {
            yield return null;
            if (_Weapon.Owner != null) {
                if (_Weapon.ItemType == ItemType.Weapon) {
                    _Weapon.Owner.WeaponController.ThrowOutMainWeapon();
                } else
                if (_Weapon.ItemType == ItemType.Vehicle) {
                    _Weapon.Owner.WeaponController.ThrowOutVehicle(_Weapon.Owner.MovementController.Rigidbody.velocity / 3f, Random.Range(-180, 540f));
                }
            }
            var itemProvider = _Weapon.gameObject.GetComponent<ItemMarkerProvider>();
            if (itemProvider != null)
                Destroy(itemProvider);
            _Weapon.WeaponView.MakeFallingDown();
        }
    }
}