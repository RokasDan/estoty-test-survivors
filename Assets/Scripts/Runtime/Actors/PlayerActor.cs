using System;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Input;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

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
        private Transform weapon;

        [SerializeField]
        private Animator animator;

        private IMovement playerMovement;
        private IPlayerInput playerInput;
        private Transform enemyTransform;
        private bool isPlayerInverted;

        private void Awake()
        {
            playerInput = new SimplePlayerInput();

            playerMovement = alwaysMaxSpeed ? new MaxSpeedMovement() : new VariableSpeedMovement();
        }

        private void Update()
        {
            var playerDirection = playerInput.GetPlayerDirection();
            playerMovement.Move(transform, playerDirection, moveSpeed);
            animator.SetBool("IsMoving", IsPlayerMoving(playerDirection));
            animator.SetBool("IsMovingBackwards", IsPlayerMovingBackwards(playerDirection));
            InvertPlayer(playerDirection);
            RotateGunArm(enemyTransform, isPlayerInverted);
        }

        private bool IsPlayerMoving(Vector2 direction)
        {
            return Mathf.Abs(direction.y) > 0 || Mathf.Abs(direction.x) > 0;
        }

        private bool IsPlayerMovingAwayFromEnemy(Vector2 joystickInput)
        {
            if (enemyTransform)
            {
                var directionToEnemy = enemyTransform.position - transform.position;
                var playerMovementDirection = joystickInput.x;
                if (playerMovementDirection > 0 && directionToEnemy.x < 0)
                {
                    return true;
                }
                if (playerMovementDirection < 0 && directionToEnemy.x > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsPlayerMovingBackwards(Vector2 joystickInput)
        {
            if (IsPlayerMoving(joystickInput) && IsPlayerMovingAwayFromEnemy(joystickInput))
            {
                return true;
            }
            return false;
        }
        private void RotateGunArm(Transform targetTransform, bool isInverted)
        {
            if (targetTransform)
            {
                var position = targetTransform.position;
                var direction = position - weapon.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (isInverted)
                {
                    angle += 180;
                }
                weapon.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        private void OnEnable()
        {
            enemyTrigger.OnTriggerEntered += OnEnemyEnterTrigger;
            enemyTrigger.OnTriggerExited += OnEnemyExitTrigger;
        }

        private void OnDisable()
        {
            enemyTrigger.OnTriggerEntered -= OnEnemyEnterTrigger;
            enemyTrigger.OnTriggerExited -= OnEnemyExitTrigger;
        }

        private void OnEnemyEnterTrigger(ColliderEnteredArgs args)
        {
            var enemy = args.Collider.transform;
            if (enemy)
            {
                enemyTransform = enemy;
            }
        }

        private void OnEnemyExitTrigger(ColliderExitedArgs args)
        {
            enemyTransform = null;
            weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        private void InvertPlayer(Vector2 joystickInput)
        {
            if (enemyTransform)
            {
                var directionToEnemy = enemyTransform.position - transform.position;
                if (directionToEnemy.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isPlayerInverted = true;
                }
                else if (directionToEnemy.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isPlayerInverted = false;
                }
            }
            else
            {
                if (joystickInput.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isPlayerInverted = true;
                }
                else if (joystickInput.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isPlayerInverted = false;
                }
            }
        }

        public Transform PlayerTransform => transform;
    }
}

