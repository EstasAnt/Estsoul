using Character.Shooting;
using Game.Movement;

namespace Game.Weapons
{
    public interface IWeaponHolder
    {
        public MovementControllerBase MovementController { get; }
        public byte Id { get; }
        public WeaponController WeaponController { get; }
    }
}