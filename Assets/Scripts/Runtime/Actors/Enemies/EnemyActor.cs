using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Data.Enemies;
using RokasDan.EstotyTestSurvivors.Runtime.Data.LootTables;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal sealed class EnemyActor : MonoBehaviour, IEnemyActor
    {
        [Header("General")]
        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        [Required]
        [SerializeField]
        private Animator enemyAnimation;

        [Required]
        [SerializeField]
        private SpriteRenderer enemySprite;

        [Header("Enemy Data")]
        [Required]
        [SerializeField]
        private EnemyData enemyData;

        [Header("Triggers")]
        [Required]
        [SerializeField]
        private ColliderTrigger attackTrigger;

        [Required]
        [SerializeField]
        private CollectibleTableData collectibleTable;

        [Inject]
        private IEnemySystem enemySystem;

        [Inject]
        private IPlayerSystem playerSystem;

        [Inject]
        private ICollectibleSystem collectibleSystem;

        private IActorPlayer actorPlayer;
        private float maxDistanceFromPlayer;
        private int currentHealth;
        private float lootDropRadius;
        private bool playerInRange;
        private float lastAttack;
        private bool enemyIsAlive;

        private void Awake()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                actorPlayer = player;
            }
            currentHealth = enemyData.MaxHealth;
            lootDropRadius = enemyData.LootDropRadius;
            maxDistanceFromPlayer = enemyData.MaxDistanceFromPlayer;
            attackTrigger.CircleCollider.radius = enemyData.AttackRange;
            enemyIsAlive = true;
        }

        private void Start()
        {
            if (!enemySystem.AliveEnemies.Contains(this))
            {
                enemySystem.TrackEnemy(this);
            }
        }

        private void Update()
        {
            if (actorPlayer is { IsPlayerDead: false } && enemyIsAlive)
            {
                FlipEnemySprite();
            }
        }

        private void FixedUpdate()
        {
            if (actorPlayer is { IsPlayerDead: false } && enemyIsAlive)
            {
                var direction = (actorPlayer.PlayerTransform.position - transform.position).normalized;
                Move(direction, enemyData.MoveSpeed);
                AttackPlayer(actorPlayer);
            }
            else
            {
                Decelerate();
            }

            EnemySelfDestruct();
        }

        private void OnEnable()
        {
            attackTrigger.OnTriggerEntered += DetectPlayer;
            attackTrigger.OnTriggerExited += LosePlayer;
        }

        private void OnDisable()
        {
            attackTrigger.OnTriggerEntered -= DetectPlayer;
            attackTrigger.OnTriggerExited -= LosePlayer;
        }

        public void DamageEnemy(int damage)
        {
            enemySprite.DOColor(Color.red, 0.2f).OnComplete(() =>
            {
                enemySprite.DORewind();
            });
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                KillEnemy();
            }
        }

        public void DamageOverTime(int tickDamage, float tickInterval, float duration)
        {
            StartCoroutine(TimeDamage(tickDamage, tickInterval, duration));
        }

        public void KillEnemy()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            actorPlayer.CurrentScoreCount += 1;
            actorPlayer.OnStatsChanged?.Invoke();
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
            return enemyData.SpawnRarity;
        }

        public Transform EnemyTransform => transform;
        public void EnemySelfDestruct()
        {
            if (actorPlayer == null)
            {
                return;
            }
            if (Vector2.Distance(actorPlayer.PlayerTransform.position, transform.position) > maxDistanceFromPlayer)
            {
                enemySystem.UntrackEnemy(this);
                Destroy(gameObject);
            }
        }

        public void AttackPlayer(IActorPlayer player)
        {
            if (playerInRange && Time.time >= lastAttack + enemyData.AttackSpeed)
            {
                enemyAnimation.SetTrigger("Hit");
                lastAttack = Time.time;
                var pushDirection = (this.actorPlayer.PlayerTransform.position - transform.position).normalized;
                player.PushPlayer(pushDirection * enemyData.PushForce);
                player.DamagePlayer(enemyData.AttackDamage);
            }
        }

        public void DetectPlayer(ColliderEnteredArgs args)
        {
            var player = args.Collider.GetComponentInParent<IActorPlayer>();
            if (player == null)
            {
                return;
            }
            playerInRange = true;
        }

        public void LosePlayer(ColliderExitedArgs args)
        {
            var player = args.Collider.GetComponentInParent<IActorPlayer>();
            if (player == null)
            {
                return;
            }
            playerInRange = false;
        }

        public void FlipEnemySprite()
        {
            if (actorPlayer != null)
            {
                bool playerOnRight = actorPlayer.PlayerTransform.position.x > transform.position.x;
                enemySprite.flipX = !playerOnRight;
            }
        }

        private IEnumerator TimeDamage(int damagePerTick, float tickInterval, float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration && currentHealth > 0)
            {
                enemySprite.DOColor(Color.green, 0.2f).OnComplete(() =>
                {
                    enemySprite.DORewind();
                });
                currentHealth -= damagePerTick;
                if (currentHealth <= 0)
                {
                    KillEnemy();
                    yield break;
                }

                yield return new WaitForSeconds(tickInterval);
                elapsedTime += tickInterval;
            }
        }

        private void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        private void Decelerate()
        {
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, Vector2.zero, 15 * Time.deltaTime);
            if (rigidBody.velocity.magnitude < 0.1f)
            {
                rigidBody.velocity = Vector2.zero;
            }
        }
    }
}
