using System.Collections.Generic;
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
        [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField]
        private bool alwaysMaxSpeed = true;

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

        private IMovement playerMovement;
        private IPlayerInput playerInput;
        private IEnemyTracker enemyTracker;
        private IWeaponController weaponController;
        private IPlayerAnimationController animationController;
        private IPlayerRotation playerInverter;

        private void Awake()
        {
            playerInput = new SimplePlayerInput();
            playerMovement = alwaysMaxSpeed ? new MaxSpeedMovement() : new VariableSpeedMovement();
            enemyTracker = new EnemyTracker(enemyTrigger);
            weaponController = new WeaponController(weapon, weaponRotation, fireSpeed);
            animationController = new PlayerAnimationController(animator);
            playerInverter = new PlayerRotation(transform);
        }

        private void Update()
        {
            var playerDirection = playerInput.GetPlayerDirection();
            playerMovement.Move(transform, playerDirection, moveSpeed);
            animationController.UpdateAnimation(playerDirection, enemyTracker.GetClosestEnemy(), transform.position);
            playerInverter.InvertPlayer(playerDirection, enemyTracker.GetClosestEnemy());
            weaponController.HandleWeaponFire(enemyTracker.GetClosestEnemy(), playerInverter.IsPlayerInverted);
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

        public Transform PlayerTransform => transform;
    }
}

