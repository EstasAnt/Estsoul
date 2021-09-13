using Core.Audio;
using UnityDI;

namespace Game.LevelSpecial
{
    public class RealWorldGateInTrigger : TriggerSignalBroadcaster<CharacterUnit, RealWorldGateInSignal>
    {

        [Dependency] private readonly AudioService _AudioService;

        public string EnterSound;

        protected override bool UseTriggerExit => false;

        protected override RealWorldGateInSignal CreateSignal(CharacterUnit unit, bool inTrigger)
        {
            if (!string.IsNullOrEmpty(EnterSound))
                _AudioService.PlaySound3D(EnterSound, false, false, transform.position);
            return new RealWorldGateInSignal()
            {
                characterUnit = unit,
                In = inTrigger,
            };
        }
    }
}