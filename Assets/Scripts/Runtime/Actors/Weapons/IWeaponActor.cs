namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons
{
    internal interface IWeaponActor
    {
        public void Shoot(int damage, float pushForce, bool isInverted);
    }
}
