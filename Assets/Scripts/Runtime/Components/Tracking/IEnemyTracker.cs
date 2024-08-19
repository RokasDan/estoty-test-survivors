using System;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Tracking
{
    internal interface IEnemyTracker
    {
        public event Action OnNoEnemiesLeft;
        public Transform GetClosestEnemy();
    }
}
