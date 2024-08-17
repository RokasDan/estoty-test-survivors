using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal sealed class EnemyActor : MonoBehaviour, IEnemyActor
    {
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

        private IPlayerActor playerActor;
        private int currentHealth;
        private bool playerInRange;
        private float lastAttack;

        private void Awake()
        {
            currentHealth = enemyData.maxHealth;
            attackTrigger.CircleCollider.radius = enemyData.attackRange;
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

        private void Update()
        {
            if (playerActor is { IsPlayerDead: false })
            {
                FlipEnemySprite();
                AttackPlayer(playerActor);
            }
        }

        private void FixedUpdate()
        {
            if (playerActor is { IsPlayerDead: false })
            {
                var direction = (playerActor.PlayerTransform.position - transform.position).normalized;
                Move(transform, direction, enemyData.moveSpeed);
            }
            else
            {
                Decelerate(enemyData.moveSpeed);
            }
        }

        public void Move(Transform actorTransform, Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        public void Decelerate(float speed)
        {
            if (rigidBody.velocity.magnitude > 0.01f)
            {
                var deceleration = rigidBody.velocity.normalized * (speed * Time.fixedDeltaTime);
                rigidBody.velocity = Vector2.MoveTowards(rigidBody.velocity, Vector2.zero, deceleration.magnitude);
            }
            else
            {
                rigidBody.velocity = Vector2.zero;
            }
        }

        public void DamageEnemy(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayerLocation(IPlayerActor player)
        {
            this.playerActor = player;
        }

        public int GetRarityLevel()
        {
            return enemyData.spawnRarity;
        }

        public void AttackPlayer(IPlayerActor player)
        {
            if (playerInRange && Time.time >= lastAttack + enemyData.attackSpeed)
            {
                enemyAnimation.SetTrigger("Hit");
                lastAttack = Time.time;
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
