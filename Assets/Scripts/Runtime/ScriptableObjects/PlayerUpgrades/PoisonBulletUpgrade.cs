using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "PoisonBulletUpgrade", menuName = "LevelUpScriptableObjects/PoisonBulletUpgrade")]
    internal sealed class PoisonBulletUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Required]
        public ProjectileActor projectile;

        public void Apply(IPlayerActor player)
        {
            if (projectile)
            {
                Debug.Log("PoisonBullet");
                player.Projectile = projectile;
            }
        }

        public float UpgradeChance => upgradeChance;
    }
}
