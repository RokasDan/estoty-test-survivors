using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.Upgrades
{
    [CreateAssetMenu(fileName = "New Upgrade_MaxHealth", menuName = "LevelUp/Upgrade_MaxHealth")]
    internal sealed class UpgradeMaxHealth : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        [SerializeField]
        private float upgradeChange = 1;

        [Min(1)]
        [SerializeField]
        private int additionalPoints;

        [Required]
        [SerializeField]
        private LevelUpText upgradeText;

        public float UpgradeChance => upgradeChange;

        public void Apply(IActorPlayer actorPlayer)
        {
            var positionAbove = new Vector2(actorPlayer.PlayerTransform.position.x, actorPlayer.PlayerTransform.position.y + 1);
            Instantiate(upgradeText, positionAbove, Quaternion.identity);
            actorPlayer.MaxPlayerHealth += additionalPoints;
        }

    }
}
