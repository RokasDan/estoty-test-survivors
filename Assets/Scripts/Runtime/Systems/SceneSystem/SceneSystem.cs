using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems.SceneSystem
{
    internal sealed class SceneSystem : MonoBehaviour, ISceneSystem
    {
        [Required]
        [SerializeField]
        private Animator crossFadeAnimator;

        public void LoadScene(int sceneIndex)
        {
            crossFadeAnimator.SetTrigger("StartFade");
            StartCoroutine(LoadLevelAfterAnimation(sceneIndex));
        }

        private IEnumerator LoadLevelAfterAnimation(int levelIndex)
        {
            yield return new WaitForEndOfFrame();
            while (!crossFadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("SceneFadeOut"))
            {
                yield return null;
            }

            while (crossFadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            SceneManager.LoadScene(levelIndex);
        }
    }
}
