using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal sealed class ActorExperience: MonoBehaviour, ICollectable
    {
        [Inject]
        private CollectibleSystem system;

        [Inject]
        private LevelSystem levelUpsystem;

        [Min(1)]
        [SerializeField]
        private float collectableLifeTime = 20;

        [Min(1)]
        [SerializeField]
        private int experienceCount = 1;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        private IActorPlayer actorPlayer;
        private float timer;

        private void Awake()
        {
            timer = collectableLifeTime;
        }

        public void Collect(IActorPlayer actorPlayer)
        {
            if (actorPlayer != null)
            {
                actorPlayer.CurrentPlayerExperience += experienceCount;
                if (actorPlayer.CurrentPlayerExperience > actorPlayer.MaxPlayerExperience)
                {
                    levelUpsystem.PlayerLevelUp();
                    actorPlayer.CurrentPlayerExperience = 0;
                }
                actorPlayer.OnStatsChanged?.Invoke();
            }
            DestroyCollectable();
        }

        private void FixedUpdate()
        {
            if (actorPlayer == null)
            {
                Decelerate();
            }
            else
            {
                var direction = (actorPlayer.PlayerTransform.position - transform.position).normalized;
                Move(direction, actorPlayer.PlayerSpeed);
            }

            CollectableSelfDestruct();
        }

        public void FallowPlayer(IActorPlayer actorPlayer)
        {
            this.actorPlayer = actorPlayer;
        }

        public void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        public void Decelerate()
        {
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, Vector2.zero, 15 * Time.deltaTime);
            if (rigidBody.velocity.magnitude < 0.1f)
            {
                rigidBody.velocity = Vector2.zero;
            }
        }

        public void DestroyCollectable()
        {
            if (system && system.ActiveCollectibles.Contains(this))
            {
                system.UntrackCollectables(this);
            }
            Destroy(gameObject);
        }

        public void CollectableSelfDestruct()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DestroyCollectable();
            }
        }
    }
}
