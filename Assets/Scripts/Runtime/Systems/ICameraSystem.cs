using RokasDan.EstotyTestSurvivors.Runtime.Actors;

namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface ICameraSystem
    {
        public void FollowPlayer(IPlayerActor player);
    }
}
