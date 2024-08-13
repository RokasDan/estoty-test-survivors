using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components
{
    internal sealed class TestRuntimeScript : MonoBehaviour
    {
        [Range(0f, 10f)]
        [SerializeField]
        private float scaleDurationSeconds = 1f;

        [SerializeField]
        private Ease scaleEase = Ease.InOutBounce;

        [Button]
        private async void Bounce()
        {
            var initialScale = transform.localScale;

            await transform
                .DOScale(initialScale / 2f, scaleDurationSeconds)
                .SetEase(scaleEase)
                .ToUniTask();

            await transform
                .DOScale(initialScale, scaleDurationSeconds)
                .SetEase(scaleEase)
                .ToUniTask();
        }
    }
}
