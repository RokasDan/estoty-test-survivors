using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    internal sealed class EnemyData : ScriptableObject
    {
        public string enemyName;
        public int maxHealth;
        public int attackDamage;
        public float attackRange;
        public float attackSpeed;
        public float moveSpeed;
        public float spawnRate;
    }
}
