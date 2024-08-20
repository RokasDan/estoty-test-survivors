using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Data.Players
{
    [CreateAssetMenu(fileName = "New PlayerData", menuName = "Players/PlayerData")]
    internal sealed class PlayerData : ScriptableObject
    {
        [Min(1)]
        [SerializeField]
        private int maxPlayerHealth = 1;

        [Min(0.1f)]
        [SerializeField]
        private float playerFireRate = 1;

        [Min(0.1f)]
        [SerializeField]
        private float playerWalkSpeed = 1;

        [Min(1)]
        [SerializeField]
        private int currentPlayerAmmo = 1;

        [Min(1)]
        [SerializeField]
        private int maxPlayerExperience = 1;

        [Min(0.1f)]
        [SerializeField]
        private float playerFireRange = 1;

        [Min(0.1f)]
        [SerializeField]
        private float playerCollectionRange = 1;

        public int MaxPlayerHealth => maxPlayerHealth;
        public float PlayerFireRate => playerFireRate;
        public float PlayerWalkSpeed => playerWalkSpeed;
        public int CurrentPlayerAmmo => currentPlayerAmmo;
        public int MaxPlayerExperience => maxPlayerExperience;
        public float PlayerFireRange => playerFireRange;
        public float PlayerCollectionRange => playerCollectionRange;
    }
}
