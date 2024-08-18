using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal interface IProjectileActor
    {
        public void MoveProjectile();
        public void DamageEnemy(ColliderEnteredArgs args);
    }
}
