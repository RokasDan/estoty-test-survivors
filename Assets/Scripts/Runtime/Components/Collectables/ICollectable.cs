using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal interface ICollectable : IMovement
    {
        public void Collect(IPlayerActor player);
        public void FallowPlayer(IPlayerActor player);
        public void DestroyCollectable();
        public void CollectableSelfDestruct();
    }
}
