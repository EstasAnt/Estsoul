using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public abstract class Trigger<T> : MonoBehaviour where T : Component
    {
        protected  List<T> _UnitsInscide = new List<T>();
        
        public IReadOnlyList<T> UnitsInside => _UnitsInscide;
        
        public bool ContainsUnit() {
            return _UnitsInscide.Count > 0;
        }

        public bool ContainsUnit(T characterUnit) {
            return _UnitsInscide.Contains(characterUnit);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<T>();
            if (!unit || _UnitsInscide.Contains(unit))
                return;
            OnUnitEnterTheTrigger(unit);
        }

        protected virtual void OnTriggerExit2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<T>();
            if (!unit || !_UnitsInscide.Contains(unit))
                return;
            OnUnitExitTheTrigger(unit);
        }

        protected virtual void OnUnitEnterTheTrigger(T unit) {
            _UnitsInscide.Add(unit);
        }

        protected virtual void OnUnitExitTheTrigger(T unit) {
            _UnitsInscide.Remove(unit);
        }
    }
}