using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Data.PlayerUpgrades;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.LevelSystem
{
    [CreateAssetMenu(fileName = "New LevelSystemSettings", menuName = "LevelUpSettings/LevelSystemSettings")]
    internal sealed class LevelSystemSettings : ScriptableObject
    {
        [Min(0)]
        public int addMaxLevelCeiling = 0;

        [Min(0)]
        public int additionalEnemies = 0;

        [Range(0f, 1f)]
        public float spawnRateModifier = 1;

        public List<ScriptableObject> playerUpgrades;

        public List<IPlayerUpgrade> GetUpgrades()
        {
            List<IPlayerUpgrade> upgradeList = new List<IPlayerUpgrade>();
            foreach (var upgrade in playerUpgrades)
            {
                if (upgrade is IPlayerUpgrade playerUpgrade)
                {
                    upgradeList.Add(playerUpgrade);
                }
            }
            return upgradeList;
        }
    }
}
