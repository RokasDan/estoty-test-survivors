using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.CollectibleSystem;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal sealed class EnemyActor : MonoBehaviour, IEnemyActor
    {
        [Inject]
        private IEnemySystem enemySystem;

        [Inject]
        private IPlayerSystem playerSystem;

        [Inject]
        private CollectibleSystem collectibleSystem;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        [Required]
        [SerializeField]
        private Animator enemyAnimation;

        [Required]
        [SerializeField]
        private SpriteRenderer enemySprite;

        [Required]
        [SerializeField]
        private EnemyData enemyData;

        [Required]
        [SerializeField]
        private ColliderTrigger attackTrigger;

        [Required]
        [SerializeField]
        private CollectibleTableData collectibleTable;

        private IPlayerActor playerActor;
        private int currentHealth;
        private float lootDropRadius;
        private bool playerInRange;
        private float lastAttack;
        private bool enemyIsAlive;

        private void Awake()
        {
            if (!enemySystem.AliveEnemies.Contains(this))
            {
                enemySystem.TrackEnemy(this);
            }
            if (playerSystem.TryGetPlayer(out var player))
            {
                playerActor = player;
            }
            currentHealth = enemyData.maxHealth;
            lootDropRadius = enemyData.lootDropRadius;
            attackTrigger.CircleCollider.radius = enemyData.attackRange;
            enemyIsAlive = true;
        }

        private void OnEnable()
        {
            attackTrigger.OnTriggerEntered += DetectPlayer;
            attackTrigger.OnTriggerExited += LosePlayer;
        }

        private void OnDestroy()
        {
            attackTrigger.OnTriggerEntered -= DetectPlayer;
            attackTrigger.OnTriggerExited -= LosePlayer;
        }

        private void Update()
        {
            if (playerActor is { IsPlayerDead: false } && enemyIsAlive)
            {
                FlipEnemySprite();
            }
        }

        private void FixedUpdate()
        {
            if (playerActor is { IsPlayerDead: false } && enemyIsAlive)
            {
                var direction = (playerActor.PlayerTransform.position - transform.position).normalized;
                Move(direction, enemyData.moveSpeed);
                AttackPlayer(playerActor);
            }
            else
            {
                Decelerate();
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

        public void DamageEnemy(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                KillEnemy();
            }
        }

        public void KillEnemy()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            playerActor.CurrentScoreCount += 1;
            playerActor.OnStatsChanged?.Invoke();
            enemySystem.UntrackEnemy(this);
            collectibleSystem.SpawnCollectables(collectibleTable, transform.position, lootDropRadius);
            enemyAnimation.SetBool("Dead", true);
            enemyIsAlive = false;
            Destroy(gameObject, 5f);
        }

        public void AddEnemySystem(EnemySystem system)
        {
            enemySystem = system;
        }

        public void PushEnemy(Vector2 force)
        {
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }

        public int GetRarityLevel()
        {
            return enemyData.spawnRarity;
        }

        public Transform EnemyTransform => transform;

        public void AttackPlayer(IPlayerActor player)
        {
            if (playerInRange && Time.time >= lastAttack + enemyData.attackSpeed)
            {
                enemyAnimation.SetTrigger("Hit");
                lastAttack = Time.time;
                var pushDirection = (playerActor.PlayerTransform.position - transform.position).normalized;
                player.PushPlayer(pushDirection * enemyData.pushForce);
                player.DamagePlayer(enemyData.attackDamage);
            }
        }

        public void DetectPlayer(ColliderEnteredArgs args)
        {
            var player = args.Collider.GetComponentInParent<IPlayerActor>();
            if (player == null)
            {
                return;
            }
            playerInRange = true;
        }

        public void LosePlayer(ColliderExitedArgs args)
        {
            var player = args.Collider.GetComponentInParent<IPlayerActor>();
            if (player == null)
            {
                return;
            }
            playerInRange = false;
        }

        public void FlipEnemySprite()
        {
            if (playerActor != null)
            {
                bool playerOnRight = playerActor.PlayerTransform.position.x > transform.position.x;
                enemySprite.flipX = !playerOnRight;
            }
        }
    }
}
