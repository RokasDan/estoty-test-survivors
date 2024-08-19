using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal interface ICollectable : IMovement
    {
        public void Collect(IActorPlayer actorPlayer);
        public void FallowPlayer(IActorPlayer actorPlayer);
        public void DestroyCollectable();
        public void CollectableSelfDestruct();
    }
}
