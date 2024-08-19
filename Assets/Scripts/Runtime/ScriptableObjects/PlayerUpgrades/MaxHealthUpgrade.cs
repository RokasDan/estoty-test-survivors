using RokasDan.EstotyTestSurvivors.Runtime.Actors;
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

        public void Apply(IPlayerActor player)
        {
            Debug.Log("MaxHealth");
            player.MaxPlayerHealth += additionalPoints;
        }

        public float UpgradeChance => upgradeChange;
    }
}
