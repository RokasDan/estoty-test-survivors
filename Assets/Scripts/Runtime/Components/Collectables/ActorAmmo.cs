using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal sealed class ActorAmmo : MonoBehaviour, ICollectable
    {
        [Inject]
        private CollectibleSystem system;

        [Min(1)]
        [SerializeField]
        private int ammoCount = 1;

        [Min(1)]
        [SerializeField]
        public float collectibleLifetime = 20f;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        private IActorPlayer actorPlayer;
        private float timer;

        private void Awake()
        {
            timer = collectibleLifetime;
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

        public void Collect(IActorPlayer player)
        {
            if (player != null)
            {
                player.CurrentPlayerAmmo += ammoCount;
                player.OnStatsChanged?.Invoke();
            }
            DestroyCollectable();
        }

        public void FallowPlayer(IActorPlayer player)
        {
            this.actorPlayer = player;
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
                Destroy(gameObject);
            }
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
    }
}
