using System.Collections.Generic;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.EnemySystem
{
    [CreateAssetMenu(fileName = "EnemySystemSettings", menuName = "ScriptableObjects/EnemySystemSettings", order = 1)]
    internal sealed class EnemySystemSettings : ScriptableObject
    {
        [Required]
        public List<EnemyActor> enemyPrefabs;
        [Min(0f)]
        public float spawnAreaWidth = 0;
        [Min(0f)]
        public float enemySpawnRate = 1;
        [Min(1)]
        public int initialEnemyCount = 1;
    }
}
