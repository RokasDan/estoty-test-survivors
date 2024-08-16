using System;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.EnemyTracker
{
    internal interface IEnemyTracker
    {
        public event Action OnNoEnemiesLeft;
        public Transform GetClosestEnemy();
    }
}
