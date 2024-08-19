using RokasDan.EstotyTestSurvivors.Runtime.Actors;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.PlayerUpgrades
{
    internal interface IPlayerUpgrade
    {
        public void Apply(IPlayerActor player);
        public float UpgradeChance { get; }
    }
}
