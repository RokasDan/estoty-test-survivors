using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader
{
    public class LevelUpText : MonoBehaviour
    {
        private void Update()
        {
            transform.Translate(Vector2.up * (2 * Time.deltaTime));
            Destroy(gameObject, 3f);
        }
    }
}
