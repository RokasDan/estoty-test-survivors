using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    internal sealed class EnemyData : ScriptableObject
    {
        public string enemyName;
        [Min(1)]
        public int maxHealth = 1;
        [Min(1)]
        public int attackDamage = 1;
        [Min(0.1f)]
        public float attackRange = 0.1f;
        [Min(0.1f)]
        public float attackSpeed = 0.1f;
        [Min(1)]
        public float pushForce = 1;
        [Min(0.1f)]
        public float moveSpeed = 0.1f;
        [Min(1)]
        public int spawnRarity = 1;
        [Min(0)]
        public float lootDropRadius = 0;
        [Min(1)]
        public float maxDistanceFromPlayer = 1;
    }
}
