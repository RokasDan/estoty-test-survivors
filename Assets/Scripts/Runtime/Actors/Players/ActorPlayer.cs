using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Projectiles;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Animation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Collectables;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Input;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Rotation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Tracking;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using RokasDan.EstotyTestSurvivors.Runtime.Data.Players;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors.Players
{
    internal sealed class ActorPlayer : MonoBehaviour, IActorPlayer
    {
        [Header("Player Data")]
        [Required]
        [SerializeField]
        private PlayerData playerData;

        [Required]
        [SerializeField]
        private Rigidbody2D rigidBody;

        [Header("Triggers")]
        [Required]
        [SerializeField]
        private ColliderTrigger enemyTrigger;

        [Required]
        [SerializeField]
        private ColliderTrigger playerBodyTrigger;

        [Required]
        [SerializeField]
        private ColliderTrigger collectableTrigger;

        [Header("Weapon settings")]
        [Required]
        [SerializeField]
        private BaseProjectileActor projectileActor;

        [Required]
        [SerializeField]
        private Transform projectileExit;

        [Required]
        [SerializeField]
        private Transform weaponRotation;

        [Header("Animation")]
        [Required]
        [SerializeField]
        private Animator animator;

        [Inject]
        private IObjectResolver objectResolver;

        [Inject]
        private ISceneSystem sceneSystem;

        private IPlayerInput playerInput;
        private IEnemyTracker enemyTracker;
        private IPlayerAnimationController animationController;
        private IPlayerRotation playerInverter;

        private float moveSpeed;
        private float fireSpeed;
        private int currentPlayerAmmo;

        private int maxPlayerHealth;
        private int currentPlayerHealth;

        private int maxPlayerExperience;
        private int currentPlayerExperience;
        private int currentPlayerScore;

        private float itemPickupSpeed;

        private readonly List<SpriteRenderer> playerSprites = new();

        private float lastShotTime;
        private bool isPlayerDead;

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

        public BaseProjectileActor ProjectileActor
        {
            get => projectileActor;
            set => projectileActor = value;
        }

        public Action OnStatsChanged { get; set; }
        public Transform PlayerTransform => transform;

        private void Awake()
        {
            playerInput = new SimplePlayerInput();
            enemyTracker = new EnemyTracker(enemyTrigger);
            animationController = new PlayerAnimationController(animator);
            playerInverter = new PlayerRotation(transform);

            maxPlayerHealth = playerData.MaxPlayerHealth;
            currentPlayerHealth = playerData.MaxPlayerHealth;

            moveSpeed = playerData.PlayerWalkSpeed;
            fireSpeed = playerData.PlayerFireRate;
            currentPlayerAmmo = playerData.CurrentPlayerAmmo;
            maxPlayerExperience = playerData.MaxPlayerExperience;

            enemyTrigger.CircleCollider.radius = playerData.PlayerFireRange;
            collectableTrigger.CircleCollider.radius = playerData.PlayerCollectionRange;

            var allSprites = GetComponentsInChildren<SpriteRenderer>();
            if (allSprites.Length != 0)
            {
                playerSprites.AddRange(allSprites);
            }
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
                    HandleWeaponFire(enemyTracker.GetClosestEnemy(), playerInverter.IsPlayerInverted);
                }
                else
                {
                    ResetWeaponPosition();
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
        private void OnDisable()
        {
            enemyTracker.OnNoEnemiesLeft -= ResetWeaponPosition;
            playerBodyTrigger.OnTriggerEntered -= CollectItem;
            collectableTrigger.OnTriggerEntered -= TrackCollectables;
            collectableTrigger.OnTriggerExited -= UntrackCollectables;
        }

        public void DamagePlayer(int damage)
        {
            if (playerSprites.Count > 0)
            {
                foreach (var sprite in playerSprites)
                {
                    sprite.DOColor(Color.red, 0.2f).OnComplete(() =>
                    {
                        sprite.DORewind();
                    });
                }
            }

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
            StartCoroutine(WaitAndLoadScene());
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

        private IEnumerator WaitAndLoadScene()
        {
            yield return new WaitForSeconds(4f);
            sceneSystem.LoadScene(2);
        }

        private void Move(Vector2 direction, float speed)
        {
            var targetVelocity = direction.normalized * speed;
            rigidBody.velocity = Vector2.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        private void Decelerate()
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

        private void FireAtEnemy(bool isInverted)
        {
            var weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + -90);
            if (isInverted)
            {
                weaponAngle = Quaternion.Euler(0, 0, projectileExit.eulerAngles.z + 90);
            }
            objectResolver.Instantiate(projectileActor, projectileExit.position, weaponAngle);
            currentPlayerAmmo -= 1;
            OnStatsChanged?.Invoke();
        }

        private void HandleWeaponFire(Transform enemyTransform, bool isPlayerInverted)
        {
            if (enemyTransform)
            {
                RotateGunArm(enemyTransform, isPlayerInverted);

                if (Time.time >= lastShotTime + fireSpeed)
                {
                    FireAtEnemy(isPlayerInverted);
                    lastShotTime = Time.time;
                }
            }
        }

        private void RotateGunArm(Transform enemyTransform, bool isInverted)
        {
            var direction = enemyTransform.position - weaponRotation.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (isInverted)
            {
                angle += 180;
            }
            weaponRotation.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void ResetWeaponPosition()
        {
            weaponRotation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
}

