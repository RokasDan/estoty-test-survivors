using RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.WeaponControl
{
    internal sealed class WeaponController : IWeaponController
    {
        private readonly WeaponActor weapon;
        private readonly Transform weaponRotation;
        private readonly float fireSpeed;
        private readonly int damage;
        private readonly float pushForce;
        private float lastShotTime;
        public WeaponController(WeaponActor weapon, Transform weaponRotation, float fireSpeed, int damage, float pushForce)
        {
            this.weapon = weapon;
            this.weaponRotation = weaponRotation;
            this.fireSpeed = fireSpeed;
            this.damage = damage;
            this.pushForce = pushForce;
        }

        public void HandleWeaponFire(Transform enemyTransform, bool isPlayerInverted)
        {
            if (enemyTransform)
            {
                RotateGunArm(enemyTransform, isPlayerInverted);

                if (Time.time >= lastShotTime + fireSpeed)
                {
                    weapon.Shoot(damage, pushForce ,isPlayerInverted);
                    lastShotTime = Time.time;
                }
            }
        }

        private void RotateGunArm(Transform enemyTransform, bool isInverted)
        {
            var direction = enemyTransform.position - weaponRotation.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (isInverted)
            {
                angle += 180;
            }
            weaponRotation.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
