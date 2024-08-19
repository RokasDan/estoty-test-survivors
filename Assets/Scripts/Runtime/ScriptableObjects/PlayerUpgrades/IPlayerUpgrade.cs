using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.PlayerUpgrades
{
    internal interface IPlayerUpgrade
    {
        public void Apply(IPlayerActor player);
        public float UpgradeChance { get; }
    }
}
