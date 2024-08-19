using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal interface ICollectable
    {
        public void Collect(IActorPlayer player);
        public void FallowPlayer(IActorPlayer player);
        public void DestroyCollectable();
    }
}
