using RokasDan.EstotyTestSurvivors.Runtime.Actors;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface IPlayerSystem
    {
        public bool TryGetPlayer(out IPlayerActor player);
    }
}
