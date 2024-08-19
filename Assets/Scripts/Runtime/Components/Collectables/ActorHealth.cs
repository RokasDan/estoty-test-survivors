using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal sealed class ActorHealth : MonoBehaviour, ICollectable
    {
        [Min(1)]
        [SerializeField]
        private float collectableLifeTime = 20;

        [Min(1)]
        [SerializeField]
        private int healthCount = 1;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        [Inject]
        private ICollectibleSystem system;

        private IActorPlayer actorPlayer;
        private float timer;

        private void Awake()
        {
            timer = collectableLifeTime;
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
            if (this.actorPlayer != null)
            {
                this.actorPlayer.CurrentPlayerHealth += healthCount;
                if (this.actorPlayer.CurrentPlayerHealth > this.actorPlayer.MaxPlayerHealth)
                {
                    this.actorPlayer.CurrentPlayerHealth = this.actorPlayer.MaxPlayerHealth;
                }
                this.actorPlayer.OnStatsChanged?.Invoke();
            }
            DestroyCollectable();
        }

        public void FallowPlayer(IActorPlayer player)
        {
            this.actorPlayer = player;
        }

        public void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        public void DestroyCollectable()
        {
            if (system != null && system.ActiveCollectibles.Contains(this))
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

        private void CollectableSelfDestruct()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DestroyCollectable();
            }
        }
    }
}
