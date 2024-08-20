using System.Collections.Generic;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.EnemySystem
{
    [CreateAssetMenu(fileName = "New EnemySystemSettings", menuName = "EnemySettings/EnemySystemSettings", order = 1)]
    internal sealed class EnemySystemSettings : ScriptableObject
    {
        [Required]
        [SerializeField]
        private List<EnemyActor> enemyPrefabs;

        [Min(0f)]
        [SerializeField]
        private int spawnAreaOffset = 0;

        [Min(0f)]
        [SerializeField]
        private float enemySpawnRate = 1;

        [Min(1)]
        [SerializeField]
        private int initialEnemyCount = 1;

        public List<EnemyActor> EnemyPrefabs
        {
            get { return enemyPrefabs; }
            set { enemyPrefabs = value; }
        }

        public int SpawnAreaOffset
        {
            get { return spawnAreaOffset; }
            set { spawnAreaOffset = value; }
        }

        public float EnemySpawnRate
        {
            get { return enemySpawnRate; }
            set { enemySpawnRate = value; }
        }

        public int InitialEnemyCount
        {
            get { return initialEnemyCount; }
            set { initialEnemyCount = value; }
        }
    }
}
