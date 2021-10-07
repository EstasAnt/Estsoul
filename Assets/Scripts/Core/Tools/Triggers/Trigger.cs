﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public abstract class Trigger<T> : MonoBehaviour
    {
        protected  List<T> _UnitsInscide = new List<T>();

        protected virtual bool UseTriggerEnter => true;
        protected virtual bool UseTriggerExit => true;
        protected virtual bool UseTriggerStay => false;
        
        public IReadOnlyList<T> UnitsInside => _UnitsInscide;
        
        public bool ContainsUnit() {
            return _UnitsInscide.Count > 0;
        }

        public bool ContainsUnit(T characterUnit) {
            return _UnitsInscide.Contains(characterUnit);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) {
            if(!UseTriggerEnter)
                return;
            var unit = col.gameObject.GetComponent<T>();
            if (unit == null || _UnitsInscide.Contains(unit))
                return;
            OnUnitEnterTheTrigger(unit);
        }

        protected virtual void OnTriggerExit2D(Collider2D col) {
            if(!UseTriggerExit)
                return;
            var unit = col.gameObject.GetComponent<T>();
            if (unit == null || !_UnitsInscide.Contains(unit))
                return;
            OnUnitExitTheTrigger(unit);
        }

        protected void OnTriggerStay2D(Collider2D col)
        {
            if(!UseTriggerStay)
                return;
            var unit = col.gameObject.GetComponent<T>();
            if (unit == null)
                return;
            OnUnitStayInTrigger(unit);
        }

        protected virtual void OnUnitEnterTheTrigger(T unit) {
            _UnitsInscide.Add(unit);
        }

        protected virtual void OnUnitExitTheTrigger(T unit) {
            _UnitsInscide.Remove(unit);
        }
        
        protected virtual void OnUnitStayInTrigger(T unit) {
            
        }
    }
}