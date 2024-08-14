using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal sealed class MaxSpeedMovement : IMovement
    {
        public void Move(Transform transform, Vector2 direction, float speed)
        {
            transform.Translate(direction.normalized * (speed * Time.deltaTime));
        }
    }
}
