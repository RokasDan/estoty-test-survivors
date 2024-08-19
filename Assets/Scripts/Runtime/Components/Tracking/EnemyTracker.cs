using System;
using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Tracking
{
    internal sealed class EnemyTracker : IEnemyTracker
    {
        private readonly List<Transform> enemies = new List<Transform>();
        private readonly ColliderTrigger enemyTrigger;
        public event Action OnNoEnemiesLeft;

        public EnemyTracker(ColliderTrigger trigger)
        {
            enemyTrigger = trigger;
            enemyTrigger.OnTriggerEntered += OnEnemyEnterTrigger;
            enemyTrigger.OnTriggerExited += OnEnemyExitTrigger;
        }

        public Transform GetClosestEnemy()
        {
            Transform closestEnemy = null;
            var closestDistanceSqr = Mathf.Infinity;
            foreach (Transform enemy in enemies)
            {
                if (!enemy)
                {
                    continue;
                }

                var distanceSqr = (enemy.position - enemyTrigger.transform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void OnEnemyEnterTrigger(ColliderEnteredArgs args)
        {
            var enemy = args.Collider.transform;
            if (enemy)
            {
                enemies.Add(enemy);
            }
        }

        private void OnEnemyExitTrigger(ColliderExitedArgs args)
        {
            var enemy = args.Collider.transform;
            if (enemy)
            {
                enemies.Remove(enemy);
            }
            if (enemies.Count == 0)
            {
                OnNoEnemiesLeft?.Invoke();
            }
        }
    }
}
