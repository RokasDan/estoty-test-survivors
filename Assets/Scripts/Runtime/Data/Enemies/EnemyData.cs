using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.Enemies
{
    [CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemies/EnemyData", order = 1)]
    internal sealed class EnemyData : ScriptableObject
    {
        [SerializeField]
        private string enemyName;
        [SerializeField, Min(1)]
        private int maxHealth = 1;
        [SerializeField, Min(1)]
        private int attackDamage = 1;
        [SerializeField, Min(0.1f)]
        private float attackRange = 0.1f;
        [SerializeField, Min(0.1f)]
        private float attackSpeed = 0.1f;
        [SerializeField, Min(1)]
        private float pushForce = 1;
        [SerializeField, Min(0.1f)]
        private float moveSpeed = 0.1f;
        [SerializeField, Min(1)]
        private int spawnRarity = 1;
        [SerializeField, Min(0)]
        private float lootDropRadius = 0;
        [SerializeField, Min(1)]
        private float maxDistanceFromPlayer = 1;

        public string EnemyName => enemyName;
        public int MaxHealth => maxHealth;
        public int AttackDamage => attackDamage;
        public float AttackRange => attackRange;
        public float AttackSpeed => attackSpeed;
        public float PushForce => pushForce;
        public float MoveSpeed => moveSpeed;
        public int SpawnRarity => spawnRarity;
        public float LootDropRadius => lootDropRadius;
        public float MaxDistanceFromPlayer => maxDistanceFromPlayer;
    }
}
