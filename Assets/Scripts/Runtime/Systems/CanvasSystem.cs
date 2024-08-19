using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal sealed class CanvasSystem : MonoBehaviour, ICanvasSystem
    {
        [Inject]
        private IPlayerSystem playerSystem;

        [Inject]
        private LevelSystem levelUpSystem;

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

        private IActorPlayer actorPlayer;

        private void Start()
        {
            if (playerSystem.TryGetPlayer(out var player))
            {
                actorPlayer = player;
                UpdatePlayerStats();
                actorPlayer.OnStatsChanged += UpdatePlayerStats;
            }
        }

        private void OnDestroy()
        {
            actorPlayer.OnStatsChanged -= UpdatePlayerStats;
        }

        private void UpdatePlayerStats()
        {
            UpdateHealth(actorPlayer.CurrentPlayerHealth, actorPlayer.MaxPlayerHealth);
            UpdateExperience(actorPlayer.CurrentPlayerExperience, actorPlayer.MaxPlayerExperience);
            UpdateKillCount(actorPlayer.CurrentScoreCount);
            UpdatedBulletCount(actorPlayer.CurrentPlayerAmmo);
            UpdatedLevelCount(levelUpSystem.CurrentPlayerLevel);
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
