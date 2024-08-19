using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.Upgrades
{
    [CreateAssetMenu(fileName = "New Upgrade_FireRate", menuName = "LevelUp/Upgrade_FireRate")]
    internal sealed class UpgradeFireRate : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Range(0f, 1f)]
        public float fireRateModifier = 1;

        [Required]
        public LevelUpText upgradeText;

        public void Apply(IActorPlayer actorPlayer)
        {
            var positionAbove = new Vector2(actorPlayer.PlayerTransform.position.x, actorPlayer.PlayerTransform.position.y + 1);
            Instantiate(upgradeText, positionAbove, Quaternion.identity);
            actorPlayer.PlayerFireRate *= fireRateModifier;
        }

        public float UpgradeChance => upgradeChance;
    }
}
