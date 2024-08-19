using RokasDan.EstotyTestSurvivors.Runtime.Actors;
using RokasDan.EstotyTestSurvivors.Runtime.Actors.Players;
using UnityEngine;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface ICameraSystem
    {
        public void FollowPlayer(IActorPlayer actorPlayer);
        public bool TryGetCamera(out Camera targetCamera);
        public Vector2 GetOutsideCameraPosition(int offset);

    }
}
