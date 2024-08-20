using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal interface IEnemyActor
    {
        public void DamageEnemy(int damage);
        public void DamageOverTime(int tickDamage, float tickInterval, float duration);
        public void PushEnemy(Vector2 force);
        public Transform EnemyTransform { get; }
    }
}
