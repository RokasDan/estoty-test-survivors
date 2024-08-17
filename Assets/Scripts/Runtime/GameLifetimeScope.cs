using NaughtyAttributes;
using RokasDan.EstotyTestSurvivors.Runtime.ScriptableObjects.EnemySystem;
using RokasDan.EstotyTestSurvivors.Runtime.Systems;
using UnityEngine;
using UnityEngine.Serialization;
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
        private CameraSystem cameraSystem;

        [FormerlySerializedAs("hudSystem")]
        [Required]
        [SerializeField]
        private CanvasSystem canvasSystem;

        [Header("Settings")]
        [Required]
        [SerializeField]
        private EnemySystemSettings enemySystemSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(playerSystem).AsImplementedInterfaces();
            builder.RegisterComponent(cameraSystem).AsImplementedInterfaces();
            builder.RegisterComponent(canvasSystem).AsImplementedInterfaces();
            builder.RegisterInstance(enemySystemSettings).AsSelf();
            builder.Register<EnemySystem>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
