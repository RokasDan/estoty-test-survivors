using UnityEngine;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class EnemySystem : IEnemySystem, IStartable
    {
        private readonly IPlayerSystem playerSystem;

        public EnemySystem(IPlayerSystem playerSystem)
        {
            this.playerSystem = playerSystem;
        }

        public void Start()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                Debug.Log($"Attack player: {player}");
            }
            else
            {
                Debug.Log($"No player found");
            }
        }
    }
}
