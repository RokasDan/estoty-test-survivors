using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables
{
    internal sealed class AmmoActor : MonoBehaviour, ICollectable
    {
        [Inject]
        private CollectibleSystem system;

        [Min(1)]
        [SerializeField]
        private int ammoCount = 1;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        private IPlayerActor playerActor;

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
        }

        public void Collect(IPlayerActor player)
        {
            if (player != null)
            {
                player.CurrentPlayerAmmo += ammoCount;
                player.OnStatsChanged?.Invoke();
            }
            DestroyCollectable();
        }

        public void FallowPlayer(IPlayerActor player)
        {
            playerActor = player;
        }

        public void DestroyCollectable()
        {
            if (system && system.ActiveCollectibles.Contains(this))
            {
                system.UntrackCollectables(this);
            }
            Destroy(gameObject);
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
