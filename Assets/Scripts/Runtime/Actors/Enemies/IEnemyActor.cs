using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal interface IEnemyActor
    {
        public void DamageEnemy(int damage);
        public void DamageOverTime(int impactDamage, int tickDamage, float tickInterval, float duration);
        public void KillEnemy();
        public void FlipEnemySprite();
        public void AddEnemySystem(EnemySystem system);
        public void AttackPlayer(IActorPlayer player);
        public void DetectPlayer(ColliderEnteredArgs args);
        public void LosePlayer(ColliderExitedArgs args);
        public void PushEnemy(Vector2 force);
        public int GetRarityLevel();
        public Transform EnemyTransform { get; }
        public void EnemySelfDestruct();
    }
}
