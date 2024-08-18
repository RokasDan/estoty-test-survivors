﻿using System;
using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class CanvasSystem : MonoBehaviour, ICanvasSystem
    {
        [Inject]
        private IPlayerSystem playerSystem;

        [Required]
        [SerializeField]
        private Slider healthSlider;

        [Required]
        [SerializeField]
        private Slider experienceSlider;

        [Required]
        [SerializeField]
        private Text scoreText;

        [Required]
        [SerializeField]
        private Text bulletText;

        [Required]
        [SerializeField]
        private Text levelText;

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

        private void OnDestroy()
        {
            playerActor.OnStatsChanged -= UpdatePlayerStats;
        }

        private void UpdatePlayerStats()
        {
            UpdateHealth(playerActor.CurrentPlayerHealth, playerActor.MaxPlayerHealth);
            UpdateExperience(playerActor.CurrentPlayerExperience, playerActor.MaxPlayerExperience);
            UpdateKillCount(playerActor.CurrentScoreCount);
            UpdatedBulletCount(playerActor.CurrentPlayerAmmo);
            UpdatedLevelCount(playerActor.CurrentPlayerLevel);
        }

        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        public void UpdateExperience(int currentExperience, int maxExperience)
        {
            experienceSlider.maxValue = maxExperience;
            experienceSlider.value = currentExperience;
        }

        public void UpdateKillCount(int count)
        {
            scoreText.text = count.ToString();
        }

        public void UpdatedBulletCount(int bullets)
        {
            bulletText.text = bullets.ToString();
        }

        public void UpdatedLevelCount(int level)
        {
            levelText.text = "Lv." + level;
        }
    }
}