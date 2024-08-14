using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Input
{
    internal sealed class SimplePlayerInput : IPlayerInput
    {
        public Vector2 GetPlayerDirection()
        {
            var horizontalInput = SimpleInput.GetAxis("Horizontal");
            var verticalInput = SimpleInput.GetAxis("Vertical");
            return new Vector2(horizontalInput, verticalInput);
        }
    }
}
