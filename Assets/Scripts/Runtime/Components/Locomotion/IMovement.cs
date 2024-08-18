using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion
{
    internal interface IMovement
    {
        public void Move(Vector2 direction, float speed);
        public void Decelerate();
    }
}
