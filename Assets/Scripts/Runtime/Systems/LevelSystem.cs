using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Data.LevelSystem;
using RokasDan.EstotyTestSurvivors.Runtime.Data.Upgrades;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class LevelSystem : MonoBehaviour, ILevelSystem
    {
        [Required]
        [SerializeField]
        private LevelSystemSettings settings;

        [Inject]
        private readonly IPlayerSystem playerSystem;

        [Inject]
        private readonly IEnemySystem enemySystem;

        public int CurrentPlayerLevel { get; private set; } = 1;

        public void PlayerLevelUp()
        {
            CurrentPlayerLevel++;
            ApplyRandomUpgrade();
            ScaleEnemySystem();
        }

        private void ApplyRandomUpgrade()
        {
            if (playerSystem.TryGetPlayer(out var playerActor))
            {
                var playerUpgrades = settings.GetUpgrades();

                if (playerUpgrades.Count == 0)
                {
                    return;
                }

                var totalDropChance = 0f;
                foreach (var upgrade in playerUpgrades)
                {
                    totalDropChance += upgrade.UpgradeChance;
                }

                var randomValue = Random.Range(0f, totalDropChance);
                var cumulativeChance = 0f;
                IPlayerUpgrade selectedUpgrade = null;

                foreach (var upgrade in playerUpgrades)
                {
                    cumulativeChance += upgrade.UpgradeChance;
                    if (randomValue <= cumulativeChance)
                    {
                        selectedUpgrade = upgrade;
                        break;
                    }
                }
                playerActor.MaxPlayerExperience += settings.addMaxLevelCeiling;
                selectedUpgrade?.Apply(playerActor);
                playerActor.OnStatsChanged.Invoke();
            }
        }

        private void ScaleEnemySystem()
        {
            enemySystem.MaxEnemyCount += settings.additionalEnemies;
            enemySystem.EnemySpawnRate *= settings.spawnRateModifier;
        }
    }
}
