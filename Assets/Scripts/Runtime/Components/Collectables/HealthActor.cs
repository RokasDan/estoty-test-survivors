using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal sealed class HealthActor : MonoBehaviour, ICollectable
    {
        [Inject]
        private CollectibleSystem system;

        [Min(1)]
        [SerializeField]
        private float collectableLifeTime = 20;

        [Min(1)]
        [SerializeField]
        private int healthCount = 1;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        private IPlayerActor playerActor;
        private float timer;

        private void Awake()
        {
            timer = collectableLifeTime;
        }

        public void Collect(IPlayerActor player)
        {
            if (playerActor != null)
            {
                playerActor.CurrentPlayerHealth += healthCount;
                if (playerActor.CurrentPlayerHealth > playerActor.MaxPlayerHealth)
                {
                    playerActor.CurrentPlayerHealth = playerActor.MaxPlayerHealth;
                }
                playerActor.OnStatsChanged?.Invoke();
            }
            DestroyCollectable();
        }

        private void FixedUpdate()
        {
            if (playerActor == null)
            {
                Decelerate();
            }
            else
            {
                var direction = (playerActor.PlayerTransform.position - transform.position).normalized;
                Move(direction, playerActor.PlayerSpeed);
            }

            CollectableSelfDestruct();
        }

        public void CollectableSelfDestruct()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DestroyCollectable();
            }
        }

        public void FallowPlayer(IPlayerActor player)
        {
            playerActor = player;
        }

        public void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        public void DestroyCollectable()
        {
            if (system && system.ActiveCollectibles.Contains(this))
            {
                system.UntrackCollectables(this);
            }
            Destroy(gameObject);
        }

        public void Decelerate()
        {
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, Vector2.zero, 15 * Time.deltaTime);
            if (rigidBody.velocity.magnitude < 0.1f)
            {
                rigidBody.velocity = Vector2.zero;
            }
        }
    }
}
