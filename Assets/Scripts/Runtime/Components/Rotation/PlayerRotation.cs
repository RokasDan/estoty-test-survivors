using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Rotation
{
    internal sealed class PlayerRotation : IPlayerRotation
    {
        private readonly Transform playerTransform;
        public bool IsPlayerInverted { get; private set; }

        public PlayerRotation(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }

        public void InvertPlayer(Vector2 joystickInput, Transform enemyTransform)
        {
            if (enemyTransform)
            {
                var directionToEnemy = enemyTransform.position - playerTransform.position;
                if (directionToEnemy.x < 0)
                {
                    playerTransform.localScale = new Vector3(-1, 1, 1);
                    IsPlayerInverted = true;
                }
                else if (directionToEnemy.x > 0)
                {
                    playerTransform.localScale = new Vector3(1, 1, 1);
                    IsPlayerInverted = false;
                }
            }
            else
            {
                if (joystickInput.x < 0)
                {
                    playerTransform.localScale = new Vector3(-1, 1, 1);
                    IsPlayerInverted = true;
                }
                else if (joystickInput.x > 0)
                {
                    playerTransform.localScale = new Vector3(1, 1, 1);
                    IsPlayerInverted = false;
                }
            }
        }
    }
}
