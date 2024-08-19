using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.Upgrades
{
    internal interface IPlayerUpgrade
    {
        public void Apply(IActorPlayer actorPlayer);
        public float UpgradeChance { get; }
    }
}
