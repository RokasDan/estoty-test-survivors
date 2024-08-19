using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "LevelUpScriptableObjects/FireRateUpgrade")]
    internal sealed class FireRateUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Range(0f, 1f)]
        public float fireRateModifier = 1;

        [Required]
        public UpgradeHeaderActor upgradeText;

        public void Apply(IPlayerActor player)
        {
            var positionAbove = new Vector2(player.PlayerTransform.position.x, player.PlayerTransform.position.y + 1);
            Instantiate(upgradeText, positionAbove, Quaternion.identity);
            player.PlayerFireRate *= fireRateModifier;
        }

        public float UpgradeChance => upgradeChance;
    }
}
