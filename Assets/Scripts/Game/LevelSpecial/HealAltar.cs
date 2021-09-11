using System;
using System.Collections;
using System.Linq;
using Character.Control;
using Character.Health;
using KlimLib.SignalBus;
using UI.Game.AltarMarker;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class HealAltar : MonoBehaviour
    {
        [Dependency] private readonly SignalBus _signalBus;
        
        public AltarMarkerProvider AltarMarkerProvider;
        public float RestoreHealthAmount;
        public float RestoreHealthTime;
        public float RestoreHealthTimePerSec;
        private void Start()
        {
            ContainerHolder.Container.BuildUp(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<CharacterUnit>();
            if(unit == null)
                return;
            AltarMarkerProvider.SetVisible(true);
            _signalBus.UnSubscribeFromAll(this);
            _signalBus.Subscribe<PlayerActionWasPressedSignal>(OnPlayerActionWasPressedSignal, this);
        }
        
        protected virtual void OnTriggerExit2D(Collider2D col) {
            var unit = col.gameObject.GetComponent<CharacterUnit>();
            if(unit == null)
                return;
            AltarMarkerProvider.SetVisible(false);
            _signalBus.UnSubscribeFromAll(this);
        }

        private void OnPlayerActionWasPressedSignal(PlayerActionWasPressedSignal signal)
        {
            StartCoroutine(RestoreHealthRoutine());
        }

        private IEnumerator RestoreHealthRoutine()
        {
            var timer = 0f;
            var restoredHealth = 0f;
            var character = CharacterUnit.Characters.FirstOrDefault();
            while (timer < RestoreHealthTime)
            {
                if(!character)
                    yield break;
                var period = 1 / RestoreHealthTimePerSec;
                var health = period / RestoreHealthTime * RestoreHealthAmount;
                health = Mathf.Clamp(health, 0, RestoreHealthAmount - restoredHealth);
                restoredHealth += health;
                timer += period;
                Debug.LogError($"RestoredHealth: {restoredHealth}");
                RestoreHealth(character, -health);
                yield return new WaitForSeconds(period);
            }
            var leftHealth = RestoreHealthAmount - restoredHealth;
            RestoreHealth(character, -leftHealth);
            Debug.LogError($"RestoredHealthFinally: {restoredHealth + leftHealth}");
            Debug.LogError($"Routine lenght {timer}");
        }

        private void RestoreHealth(CharacterUnit unit, float amounth)
        {
            var dmg = new Damage(null, unit, amounth);
            unit.ApplyDamage(dmg);
        }
        
        private void OnDestroy()
        {
            _signalBus.UnSubscribeFromAll(this);
        }
    }
}