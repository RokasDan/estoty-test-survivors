using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal sealed class ProjectileActor : MonoBehaviour, IProjectileActor
    {
        [Min(0.1f)]
        [SerializeField]
        private float projectileSpeed = 0.1f;

        [Required]
        [SerializeField]
        private ColliderTrigger enemyTrigger;

        private int projectileDamage;
        private float projectileForce;

        private void OnEnable()
        {
            enemyTrigger.OnTriggerEntered += DamageEnemy;
        }

        private void OnDisable()
        {
            enemyTrigger.OnTriggerEntered -= DamageEnemy;
        }

        public void MoveProjectile()
        {
            transform.Translate(Vector2.up * (projectileSpeed * Time.deltaTime));
        }

        public void SetProjectileStats(int damage, float pushForce)
        {
            projectileDamage = damage;
            projectileForce = pushForce;
        }

        public void DamageEnemy(ColliderEnteredArgs args)
        {
            var enemyActor = args.Collider.GetComponentInParent<IEnemyActor>();
            if (enemyActor == null)
            {
                return;
            }
            var pushDirection = (enemyActor.EnemyTransform.position - transform.position).normalized;
            enemyActor.DamageEnemy(projectileDamage);
            enemyActor.PushEnemy(pushDirection * projectileForce);
            Destroy(gameObject);
        }

        private void Update()
        {
            MoveProjectile();
        }
    }
}
