using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal abstract class BaseProjectileActor : MonoBehaviour, IProjectileActor
    {
        [Min(0)]
        [SerializeField]
        protected float projectileSpeed = 0.1f;

        [Min(1)]
        [SerializeField]
        protected int projectileDamage = 1;

        [Min(1)]
        [SerializeField]
        protected float pushForce = 1;

        [Min(1)]
        [SerializeField]
        protected float projectileLifeTime = 10;

        [Required]
        [SerializeField]
        protected ColliderTrigger enemyTrigger;

        private float timer;

        private void Awake()
        {
            timer = projectileLifeTime;
        }

        protected virtual void OnEnable()
        {
            enemyTrigger.OnTriggerEntered += DamageEnemy;
        }

        protected virtual void OnDestroy()
        {
            enemyTrigger.OnTriggerEntered -= DamageEnemy;
        }

        public virtual void MoveProjectile()
        {
            transform.Translate(Vector2.up * (projectileSpeed * Time.deltaTime));
        }

        public abstract void DamageEnemy(ColliderEnteredArgs args);
        public void ProjectileSelfDestruct()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Update()
        {
            MoveProjectile();
            ProjectileSelfDestruct();
        }
    }
}
