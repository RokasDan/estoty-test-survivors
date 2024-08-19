using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Animation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables;
using RokasDan.EstotyTestSurvivors.Runtime.Components.EnemyTracker;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Input;
using RokasDan.EstotyTestSurvivors.Runtime.Components.PlayerRotation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Components.WeaponControl;
using UnityEngine;


namespace RokasDan.EstotyTestSurvivors.Runtime.Actors
{
    internal sealed class PlayerActor : MonoBehaviour, IPlayerActor
    {
        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        [SerializeField]
        private float moveSpeed = 5f;

        [Required]
        [SerializeField]
        private ColliderTrigger enemyTrigger;

        [Required]
        [SerializeField]
        private ColliderTrigger playerBodyTrigger;

        [Required]
        [SerializeField]
        private ColliderTrigger collectableTrigger;

        [SerializeField]
        private Transform weaponRotation;

        [SerializeField]
        private WeaponActor weapon;

        [SerializeField]
        private float fireSpeed = 2;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private ProjectileActor projectileActor;

        private IPlayerInput playerInput;
        private IEnemyTracker enemyTracker;
        private IWeaponController weaponController;
        private IPlayerAnimationController animationController;
        private IPlayerRotation playerInverter;

        private int currentPlayerAmmo = 20;
        private int maxPlayerHealth = 10;
        private int currentPlayerHealth = 0;
        private int maxPlayerExperience = 15;
        private int currentPlayerExperience = 0;
        private int currentPlayerLevel = 0;
        private int currentPlayerScore = 0;
        private float itemPickupSpeed = 2;

        private int playerDamage = 1;
        private float playerPushForce = 15;

        private bool isPlayerDead;

        private void Awake()
        {
            playerInput = new SimplePlayerInput();
            enemyTracker = new EnemyTracker(enemyTrigger);
            weaponController = new WeaponController(weapon, weaponRotation, fireSpeed, playerDamage, playerPushForce);
            animationController = new PlayerAnimationController(animator);
            playerInverter = new PlayerRotation(transform);
            currentPlayerHealth = maxPlayerHealth;
        }

        private void Update()
        {
            if (!IsPlayerDead)
            {
                var playerDirection = playerInput.GetPlayerDirection();
                animationController.UpdateAnimation(playerDirection, enemyTracker.GetClosestEnemy(), transform.position);
                playerInverter.InvertPlayer(playerDirection, enemyTracker.GetClosestEnemy());
                if (CurrentPlayerAmmo > 0)
                {
                    weaponController.HandleWeaponFire(enemyTracker.GetClosestEnemy(), playerInverter.IsPlayerInverted);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!IsPlayerDead)
            {
                var playerDirection = playerInput.GetPlayerDirection();
                Move(playerDirection, moveSpeed);
            }
            else
            {
                Decelerate();
            }
        }

        private void OnEnable()
        {
            enemyTracker.OnNoEnemiesLeft += ResetWeaponPosition;
            playerBodyTrigger.OnTriggerEntered += CollectItem;
            collectableTrigger.OnTriggerEntered += TrackCollectables;
            collectableTrigger.OnTriggerExited += UntrackCollectables;
        }
        private void OnDestroy()
        {
            enemyTracker.OnNoEnemiesLeft -= ResetWeaponPosition;
            playerBodyTrigger.OnTriggerEntered -= CollectItem;
            collectableTrigger.OnTriggerEntered -= TrackCollectables;
            collectableTrigger.OnTriggerExited -= UntrackCollectables;
        }

        private void ResetWeaponPosition()
        {
            weaponRotation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void DamagePlayer(int damage)
        {
            currentPlayerHealth -= damage;
            OnStatsChanged?.Invoke();
            if (currentPlayerHealth < 0)
            {
                KillPlayer();
            }
        }

        public void KillPlayer()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            animationController.ShowPlayerDeath();
            isPlayerDead = true;
        }

        public void PushPlayer(Vector2 force)
        {
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }

        public void TrackCollectables(ColliderEnteredArgs args)
        {
            var collectible = args.Collider.GetComponentInParent<ICollectable>();
            if (collectible == null)
            {
                return;
            }
            collectible.FallowPlayer(this);
        }

        public void UntrackCollectables(ColliderExitedArgs args)
        {
            var collectible = args.Collider.GetComponentInParent<ICollectable>();
            if (collectible == null)
            {
                return;
            }
            collectible.FallowPlayer(null);
        }

        public void CollectItem(ColliderEnteredArgs args)
        {
            var collectible = args.Collider.GetComponentInParent<ICollectable>();
            if (collectible == null)
            {
                return;
            }
            collectible.Collect(this);
        }

        public bool IsPlayerDead => isPlayerDead;

        public int MaxPlayerHealth
        {
            get => maxPlayerHealth;
            set => maxPlayerHealth = value;
        }

        public int CurrentPlayerHealth
        {
            get => currentPlayerHealth;
            set => currentPlayerHealth = value;
        }

        public int MaxPlayerExperience
        {
            get => maxPlayerExperience;
            set => maxPlayerExperience = value;
        }

        public int CurrentPlayerExperience
        {
            get => currentPlayerExperience;
            set => currentPlayerExperience = value;
        }

        public int CurrentPlayerLevel
        {
            get => currentPlayerLevel;
            set => currentPlayerLevel = value;
        }

        public int CurrentPlayerDamage
        {
            get => playerDamage;
            set => playerDamage = value;
        }

        public int CurrentPlayerAmmo
        {
            get => currentPlayerAmmo;
            set => currentPlayerAmmo = value;
        }

        public int CurrentScoreCount
        {
            get => currentPlayerScore;
            set => currentPlayerScore = value;
        }

        public float PlayerPushForce
        {
            get => playerPushForce;
            set => playerPushForce = value;
        }

        public float PlayerSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        public float PlayerFireRate
        {
            get => fireSpeed;
            set => fireSpeed = value;
        }

        public ProjectileActor Projectile
        {
            get => projectileActor;
            set => projectileActor = value;
        }

        public Action OnStatsChanged { get; set; }
        public Transform PlayerTransform => transform;

        public void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        public void Decelerate()
        {
            if (rigidBody.velocity.magnitude > 0.01f)
            {
                var deceleration = rigidBody.velocity.normalized * (moveSpeed * Time.fixedDeltaTime);
                rigidBody.velocity = Vector2.MoveTowards(rigidBody.velocity, Vector2.zero, deceleration.magnitude);
            }
            else
            {
                rigidBody.velocity = Vector2.zero;
            }
        }
    }
}

