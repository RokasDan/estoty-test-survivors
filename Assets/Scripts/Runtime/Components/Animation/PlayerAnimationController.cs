using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Animation
{
    internal sealed class PlayerAnimationController : IPlayerAnimationController
    {
        private readonly Animator animator;

        public PlayerAnimationController(Animator animator)
        {
            this.animator = animator;
        }

        public void UpdateAnimation(Vector2 playerDirection, Transform enemyTransform, Vector3 playerPosition)
        {
            animator.SetBool("IsMoving", IsPlayerMoving(playerDirection));
            animator.SetBool("IsMovingBackwards", IsPlayerMovingBackwards(playerDirection, enemyTransform, playerPosition));
        }

        public void ShowPlayerDeath()
        {
            animator.SetBool("IsDead", true);
        }

        private bool IsPlayerMoving(Vector2 direction)
        {
            return Mathf.Abs(direction.y) > 0 || Mathf.Abs(direction.x) > 0;
        }

        private bool IsPlayerMovingBackwards(Vector2 joystickInput, Transform enemyTransform, Vector3 playerPosition)
        {
            if (enemyTransform)
            {
                var directionToEnemy = enemyTransform.position - playerPosition;
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
    }
}
