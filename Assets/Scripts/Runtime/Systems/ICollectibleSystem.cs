using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.CollectibleSystem;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface ICollectibleSystem
    {
        public void SpawnCollectables(CollectibleTableData collectibleTable, Vector2 position, float spreadRadius);
        public void TrackCollectables(ICollectable collectable);
        public void UntrackCollectables(ICollectable collectable);
        public void DestroyAllCollectables();
        public List<ICollectable> ActiveCollectibles { get; }
    }
}
