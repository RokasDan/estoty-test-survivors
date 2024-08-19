using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons
{
    internal sealed class WeaponActor : MonoBehaviour, IWeaponActor
    {
        [Inject]
        private IObjectResolver objectResolver;

        [Required]
        [SerializeField]
        private SpriteRenderer weapon;

        [Required]
        [SerializeField]
        private Transform projectileExit;

        [FormerlySerializedAs("projectile")]
        [Required]
        [SerializeField]
        private RifleProjectileActor rifleProjectile;

        public void Shoot(int damage, float pushForce, bool isInverted)
        {
            var weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + -90);
            if (isInverted)
            {
                weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + 90);
            }
            objectResolver.Instantiate(rifleProjectile, projectileExit.position, weaponAngle);
        }
    }
}
