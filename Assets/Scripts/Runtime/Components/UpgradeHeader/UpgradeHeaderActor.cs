using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.UpgradeHeader
{
    public class UpgradeHeaderActor : MonoBehaviour
    {
        void Update()
        {
            transform.Translate(Vector2.up * (2 * Time.deltaTime));
            Destroy(gameObject, 3f);
        }
    }
}
