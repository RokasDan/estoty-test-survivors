using System;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class CanvasSystem : MonoBehaviour, ICanvasSystem
    {
        [Inject]
        private IPlayerSystem playerSystem;

        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Slider experienceSlider;

        [SerializeField]
        private Text scoreText;

        private IPlayerActor playerActor;

        private void Start()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                playerActor = player;
                UpdatePlayerStats();
                playerActor.OnStatsChanged += UpdatePlayerStats;
            }
        }

        private void OnDisable()
        {
            playerActor.OnStatsChanged -= UpdatePlayerStats;
        }

        private void UpdatePlayerStats()
        {
            UpdateHealth(playerActor.CurrentPlayerHealth, playerActor.MaxPlayerHealth);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        public void UpdateExperience(int currentExperience)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateKillCount(int count)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatedBulletCount(int bullets)
        {
            throw new System.NotImplementedException();
        }
    }
}
