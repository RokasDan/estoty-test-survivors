using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.Data.EnemySystem;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RokasDan.EstotyTestSurvivors.Runtime
{
    internal sealed class GameLifetimeScope : LifetimeScope
    {
        [Header("Systems")]
        [Required]
        [SerializeField]
        private PlayerSystem playerSystem;

        [Required]
        [SerializeField]
        private SceneSystem sceneSystem;

        [Required]
        [SerializeField]
        private LevelSystem levelSystem;

        [Required]
        [SerializeField]
        private CameraSystem cameraSystem;

        [Required]
        [SerializeField]
        private CanvasSystem canvasSystem;

        [Required]
        [SerializeField]
        private CollectibleSystem collectibleSystem;

        [Header("Settings")]
        [Required]
        [SerializeField]
        private EnemySystemSettings enemySystemSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneSystem).AsImplementedInterfaces();
            builder.RegisterComponent(playerSystem).AsImplementedInterfaces();
            builder.RegisterComponent(cameraSystem).AsImplementedInterfaces();
            builder.RegisterComponent(canvasSystem).AsImplementedInterfaces();
            builder.RegisterComponent(collectibleSystem).AsImplementedInterfaces();
            builder.RegisterInstance(enemySystemSettings).AsSelf();
            builder.Register<EnemySystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(levelSystem).AsImplementedInterfaces();
        }
    }
}
