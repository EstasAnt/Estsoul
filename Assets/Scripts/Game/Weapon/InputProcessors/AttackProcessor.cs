namespace Character.Shooting
{
    public class AttackProcessor : WeaponInputProcessor
    {
        public AttackProcessor(Weapon weapon) : base(weapon) { }
        
        public override void ProcessPress() {
            Weapon.PerformShot();
            base.ProcessPress();
        }
    }
}