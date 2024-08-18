using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal sealed class ProjectileActor : MonoBehaviour, IProjectileActor
    {
        [Inject]
        private IPlayerSystem playerSystem;

        [Min(0.1f)]
        [SerializeField]
        private float projectileSpeed = 0.1f;

        [Required]
        [SerializeField]
        private ColliderTrigger enemyTrigger;

        private IPlayerActor playerActor;

        private void Awake()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                playerActor = player;
                playerActor.CurrentPlayerAmmo -= 1;
                playerActor.OnStatsChanged?.Invoke();
            }
        }

        private void OnEnable()
        {
            enemyTrigger.OnTriggerEntered += DamageEnemy;
        }

        private void OnDestroy()
        {
            enemyTrigger.OnTriggerEntered -= DamageEnemy;
        }

        public void MoveProjectile()
        {
            transform.Translate(Vector2.up * (projectileSpeed * Time.deltaTime));
        }

        public void DamageEnemy(ColliderEnteredArgs args)
        {
            var enemyActor = args.Collider.GetComponentInParent<IEnemyActor>();
            if (enemyActor == null)
            {
                return;
            }
            var pushDirection = (enemyActor.EnemyTransform.position - transform.position).normalized;
            enemyActor.DamageEnemy(playerActor.CurrentPlayerDamage);
            enemyActor.PushEnemy(pushDirection * playerActor.PlayerPushForce);
            Destroy(gameObject);
        }

        private void Update()
        {
            MoveProjectile();
        }
    }
}
