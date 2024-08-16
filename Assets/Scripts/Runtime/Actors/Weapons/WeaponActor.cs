using NaughtyAttributes;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons
{
    internal sealed class WeaponActor : MonoBehaviour, IWeaponActor
    {
        [Required]
        [SerializeField]
        private SpriteRenderer weapon;

        [Required]
        [SerializeField]
        private Transform projectileExit;

        [Required]
        [SerializeField]
        private GameObject projectile;

        public void Shoot(bool isInverted)
        {
            var weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + -90);
            if (isInverted)
            {
                weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + 90);
            }
            Instantiate(projectile, projectileExit.position, weaponAngle);
        }
    }
}
