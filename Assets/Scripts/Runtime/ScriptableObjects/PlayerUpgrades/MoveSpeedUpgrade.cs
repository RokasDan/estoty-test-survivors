using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "LevelUpScriptableObjects/MoveSpeedUpgrade")]
    internal sealed class MoveSpeedUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Range(1f, 2f)]
        public float speedModifier = 1;

        [Required]
        public UpgradeHeaderActor upgradeText;

        public void Apply(IPlayerActor player)
        {
            var positionAbove = new Vector2(player.PlayerTransform.position.x, player.PlayerTransform.position.y + 1);
            Instantiate(upgradeText, positionAbove, Quaternion.identity);
            player.PlayerSpeed *= speedModifier;
        }

        public float UpgradeChance => upgradeChance;
    }
}
