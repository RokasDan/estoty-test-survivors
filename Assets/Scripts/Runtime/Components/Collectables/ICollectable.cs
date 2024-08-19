using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal interface ICollectable : IMovement
    {
        public void Collect(IActorPlayer player);
        public void FallowPlayer(IActorPlayer player);
        public void DestroyCollectable();
        public void CollectableSelfDestruct();
    }
}
