using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal sealed class PoisonProjectileActor : BaseProjectileActor
    {
        [Min(1)]
        [SerializeField]
        public float damageDuration = 1;

        [Min(0.1f)]
        [SerializeField]
        public float damageInterval = 1;

        public override void DamageEnemy(ColliderEnteredArgs args)
        {
            var enemyActor = args.Collider.GetComponentInParent<IEnemyActor>();
            if (enemyActor == null)
            {
                return;
            }
            var pushDirection = (enemyActor.EnemyTransform.position - transform.position).normalized;
            enemyActor.PushEnemy(pushDirection * pushForce);
            enemyActor.DamageOverTime(projectileDamage, damageInterval, damageDuration);
            Destroy(gameObject);
        }
    }
}
