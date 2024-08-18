using System;
using System.Collections.Generic;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Weapons;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Animation;
using RokasDan.EstotyTestSurvivors.Runtime.Components.EnemyTracker;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Input;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
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

        [SerializeField]
        private ColliderTrigger enemyTrigger;

        [SerializeField]
        private Transform weaponRotation;

        [SerializeField]
        private WeaponActor weapon;

        [SerializeField]
        private float fireSpeed = 2;

        [SerializeField]
        private Animator animator;

        private IPlayerInput playerInput;
        private IEnemyTracker enemyTracker;
        private IWeaponController weaponController;
        private IPlayerAnimationController animationController;
        private IPlayerRotation playerInverter;

        private int playerMaxHealth = 10;
        private int currentHealth;
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
            currentHealth = playerMaxHealth;
        }

        private void Update()
        {
            if (!IsPlayerDead)
            {
                var playerDirection = playerInput.GetPlayerDirection();
                animationController.UpdateAnimation(playerDirection, enemyTracker.GetClosestEnemy(), transform.position);
                playerInverter.InvertPlayer(playerDirection, enemyTracker.GetClosestEnemy());
                weaponController.HandleWeaponFire(enemyTracker.GetClosestEnemy(), playerInverter.IsPlayerInverted);
            }
        }

        private void FixedUpdate()
        {
            if (!IsPlayerDead)
            {
                var playerDirection = playerInput.GetPlayerDirection();
                Move(transform, playerDirection, moveSpeed);
            }
            else
            {
                Decelerate(moveSpeed);
            }
        }

        private void OnEnable()
        {
            enemyTracker.OnNoEnemiesLeft += ResetWeaponPosition;
        }
        private void OnDestroy()
        {
            enemyTracker.OnNoEnemiesLeft -= ResetWeaponPosition;
        }

        private void ResetWeaponPosition()
        {
            weaponRotation.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void DamagePlayer(int damage)
        {
            currentHealth -= damage;
            OnStatsChanged?.Invoke();
            if (currentHealth < 0)
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

        public bool IsPlayerDead => isPlayerDead;
        public int MaxPlayerHealth => playerMaxHealth;
        public int CurrentPlayerHealth => currentHealth;
        public int CurrentPlayerExperience { get; }
        public int BulletCount { get; }
        public int ScoreCount { get; }
        public event Action OnStatsChanged;
        public Transform PlayerTransform => transform;
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
    }
}

