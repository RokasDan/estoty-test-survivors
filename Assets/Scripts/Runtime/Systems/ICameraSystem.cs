using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface ICameraSystem
    {
        public void FollowPlayer(IPlayerActor player);
        public bool TryGetCamera(out Camera targetCamera);
    }
}
