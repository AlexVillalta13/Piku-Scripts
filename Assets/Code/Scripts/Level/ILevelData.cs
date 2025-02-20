public interface ILevelData
{
    public void SetCurrentEnemyToCheckPoint();
    public void NextEnemy();
    public SceneEnum GetEnvironment();
    
    public EnemyData GetEnemy(int enemyNumber);
    public int GetCurrentEnemy();
    public int GetTotalEnemiesCount();
    public void LoadLevelData();
}