using System;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles
{
    internal sealed class ProjectileActor : MonoBehaviour, IProjectileActor
    {
        [SerializeField]
        public float projectileSpeed = 0;

        public void MoveProjectile()
        {
            transform.Translate(Vector2.up * (projectileSpeed * Time.deltaTime));
        }

        private void Update()
        {
            MoveProjectile();
        }
    }
}
