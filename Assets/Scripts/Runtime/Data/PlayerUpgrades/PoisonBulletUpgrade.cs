using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.PlayerUpgrades
{
    [CreateAssetMenu(fileName = "PoisonBulletUpgrade", menuName = "LevelUpScriptableObjects/PoisonBulletUpgrade")]
    internal sealed class PoisonBulletUpgrade : ScriptableObject, IPlayerUpgrade
    {
        [Range(0f, 1f)]
        public float upgradeChance = 1;

        [Range(1f, 2f)]
        public float damageTimeModifier = 1;

        [Required]
        public PoisonProjectileActor poisonProjectileActor;

        [Required]
        public UpgradeHeaderActor upgradeText;

        public void Apply(IPlayerActor player)
        {
            if (poisonProjectileActor)
            {
                var positionAbove = new Vector2(player.PlayerTransform.position.x, player.PlayerTransform.position.y + 1);
                Instantiate(upgradeText, positionAbove, Quaternion.identity);
                if (player.ProjectileActor is PoisonProjectileActor poisonProjectile)
                {
                    poisonProjectile.damageDuration *= damageTimeModifier;
                }
                else
                {
                    player.ProjectileActor = poisonProjectileActor;
                }
            }
        }

        public float UpgradeChance => upgradeChance;
    }
}
