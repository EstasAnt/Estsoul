using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial
{
    public abstract class TriggerSignalBroadcaster<T, S> : Trigger<T> where T : Component
    {
        [Dependency]
        protected readonly SignalBus _SignalBus;

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected override void OnUnitEnterTheTrigger(T  unit) {
            base.OnUnitEnterTheTrigger(unit);
            _SignalBus.FireSignal(CreateSignal(unit, true));
        }

        protected override void OnUnitExitTheTrigger(T  unit) {
            base.OnUnitExitTheTrigger(unit);
            _SignalBus.FireSignal(CreateSignal(unit, false));
        }

        protected override void OnUnitStayInTrigger(T unit)
        {
            base.OnUnitStayInTrigger(unit);
            _SignalBus.FireSignal(CreateSignal(unit, true));
        }

        protected abstract S CreateSignal(T unit, bool inTrigger);
    }
}