using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface IEnemySystem
    {
        public void SpawnEnemy(EnemyActor enemyPrefab);
        public EnemyActor GetEnemyPrefab();
        public void TrackEnemy(EnemyActor enemyActor);
        public void UntrackEnemy(EnemyActor enemyActor);
    }
}
