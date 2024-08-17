using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    internal sealed class EnemyData : ScriptableObject
    {
        public string enemyName;
        [Min(1)]
        public int maxHealth;
        [Min(1)]
        public int attackDamage;
        [Min(0)]
        public float attackRange;
        [Min(0)]
        public float attackSpeed;
        [Min(0)]
        public float moveSpeed;
        [Min(1)]
        public int spawnRarity;
    }
}
