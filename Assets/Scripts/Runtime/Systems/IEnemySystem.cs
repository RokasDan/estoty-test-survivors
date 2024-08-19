using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface IEnemySystem
    {
        public void SpawnEnemy(EnemyActor enemyPrefab);
        public EnemyActor GetEnemyPrefab();
        public void TrackEnemy(IEnemyActor enemyActor);
        public void UntrackEnemy(IEnemyActor enemyActor);
        public int MaxEnemyCount { get; set; }
        public float EnemySpawnRate { get; set; }
        public List<IEnemyActor> AliveEnemies { get; }
    }
}
