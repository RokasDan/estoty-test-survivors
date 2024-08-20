using System.Collections.Generic;
using RokasDan.EstotyTestSurvivors.Runtime.Data.Upgrades;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.LevelSystem
{
    [CreateAssetMenu(fileName = "New LevelSystemSettings", menuName = "LevelUpSettings/LevelSystemSettings")]
    internal sealed class LevelSystemSettings : ScriptableObject
    {
        [Min(0)]
        [SerializeField]
        private int addMaxLevelCeiling;

        [Min(0)]
        [SerializeField]
        private int additionalEnemies;

        [Range(0f, 1f)]
        [SerializeField]
        private float spawnRateModifier = 1;

        public List<ScriptableObject> playerUpgrades;

        public int AddMaxLevelCeiling
        {
            get { return addMaxLevelCeiling; }
            set { addMaxLevelCeiling = value; }
        }

        public int AdditionalEnemies
        {
            get { return additionalEnemies; }
            set { additionalEnemies = value; }
        }

        public float SpawnRateModifier
        {
            get { return spawnRateModifier; }
            set { spawnRateModifier = value; }
        }

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
