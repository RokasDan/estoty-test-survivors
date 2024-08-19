using RokasDan.EstotyTestSurvivors.Runtime.Actors;
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

        public void Apply(IPlayerActor player)
        {
            Debug.Log("MoveSpeed");
            player.PlayerSpeed *= speedModifier;
        }

        public float UpgradeChance => upgradeChance;
    }
}
