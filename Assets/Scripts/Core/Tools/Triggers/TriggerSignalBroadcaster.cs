using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial
{
    public abstract class TriggerSignalBroadcaster<T, S> : Trigger<T>
    {
        [Dependency]
        protected readonly SignalBus _SignalBus;

        protected virtual void Start() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        protected override void OnUnitEnterTheTrigger(T  unit) {
            if(_SignalBus == null)
                ContainerHolder.Container.BuildUp(GetType(), this);
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