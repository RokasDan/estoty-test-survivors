using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;
using UnityEngine.Serialization;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "MaxHealthUpgrade", menuName = "LevelUpScriptableObjects/MaxHealthUpgrade")]
    internal sealed class MaxHealthUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChange = 1;

        [Min(1)]
        public int additionalPoints;

        [Required]
        public UpgradeHeaderActor upgradeText;

        public void Apply(IPlayerActor player)
        {
            var positionAbove = new Vector2(player.PlayerTransform.position.x, player.PlayerTransform.position.y + 1);
            Instantiate(upgradeText, positionAbove, Quaternion.identity);
            player.MaxPlayerHealth += additionalPoints;
        }

        public float UpgradeChance => upgradeChange;
    }
}
