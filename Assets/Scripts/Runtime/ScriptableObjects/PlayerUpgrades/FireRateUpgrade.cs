using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "LevelUpScriptableObjects/FireRateUpgrade")]
    internal sealed class FireRateUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Range(0f, 1f)]
        public float fireRateModifier = 1;
        public void Apply(IPlayerActor player)
        {
            Debug.Log("FireRate");
            player.PlayerFireRate *= fireRateModifier;
        }

        public float UpgradeChance => upgradeChance;
    }
}
