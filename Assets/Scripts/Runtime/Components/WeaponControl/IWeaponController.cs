using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.WeaponControl
{
    internal interface IWeaponController
    {
       public void HandleWeaponFire(Transform enemyTransform, bool isPlayerInverted);
    }
}
