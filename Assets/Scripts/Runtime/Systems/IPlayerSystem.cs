using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface IPlayerSystem
    {
        public bool TryGetPlayer(out IActorPlayer actorPlayer);
    }
}
