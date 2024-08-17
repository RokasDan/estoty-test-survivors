using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.Enemies;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Enemies
{
    internal sealed class EnemyActor : MonoBehaviour, IEnemyActor
    {
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

        private Transform player;
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
            IPlayerActor playerActor = FindObjectOfType<PlayerActor>();
            if (playerActor != null)
            {
                player = playerActor.PlayerTransform;
            }
            else
            {
                Debug.LogWarning("No IPlayerActor found in the scene.");
            }

            if (player)
            {
                Move(transform ,player.position ,enemyData.moveSpeed);
                FlipEnemySprite();
                AttackPlayer(playerActor);
            }
        }

        public void Move(Transform actorTransform, Vector2 direction, float speed)
        {
            actorTransform.position = Vector2.MoveTowards(actorTransform.position, direction, speed * Time.deltaTime);
        }

        public void DamageEnemy(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayerLocation(IPlayerActor playerActor)
        {
            player = playerActor.PlayerTransform;
        }

        public int GetRarityLevel()
        {
            return enemyData.spawnRarity;
        }

        public void AttackPlayer(IPlayerActor playerActor)
        {
            if (playerInRange && Time.time >= lastAttack + enemyData.attackSpeed)
            {
                enemyAnimation.SetTrigger("Hit");
                lastAttack = Time.time;
            }
        }

        public void DetectPlayer(ColliderEnteredArgs args)
        {
            var player = args.Collider.GetComponentInParent<IPlayerActor>();
            if (player == null)
            {
                return;
            }
            Debug.Log("Player found");
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
            if (player)
            {
                bool playerOnRight = player.position.x > transform.position.x;
                enemySprite.flipX = !playerOnRight;
            }
        }
    }
}
