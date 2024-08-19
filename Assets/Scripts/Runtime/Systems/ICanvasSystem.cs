namespace RokasDan.EstotyTestSurvivors.Runtime.Systems
{
    internal interface ICanvasSystem
    {
        public void UpdateHealth(int currentHealth, int maxHealth);
        public void UpdateExperience(int currentExperience, int maxExperience);
        public void UpdateKillCount(int count);
        public void UpdatedBulletCount(int bullets);
        public void UpdatedLevelCount(int level);
    }
}
