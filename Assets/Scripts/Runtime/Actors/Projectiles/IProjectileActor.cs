using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal interface IProjectileActor
    {
        public void MoveProjectile();
        public void SetProjectileStats(int damage, float pushForce);
        public void DamageEnemy(ColliderEnteredArgs args);
    }
}
