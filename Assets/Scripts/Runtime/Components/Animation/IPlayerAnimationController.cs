using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Animation
{
    internal interface IPlayerAnimationController
    {
        public void UpdateAnimation(Vector2 playerDirection, Transform enemyTransform, Vector3 playerPosition);
    }
}
