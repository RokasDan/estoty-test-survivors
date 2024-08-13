using NaughtyAttributes;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(playerSystem).AsImplementedInterfaces();
            builder.Register<EnemySystem>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
