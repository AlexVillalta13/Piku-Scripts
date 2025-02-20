using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBarPresenter : MonoBehaviour
{
    private bool inCombat = false;
    private bool canSpawnBricks = true;

    private float minTimeToSpawnEnemyBrick = 0f;
    private float maxTimeToSpawnEnemyBrick = 0f;
    private float timeToSpawnEnemyBrick = 0f;
    private float spawnEnemyBrickTimer = 0f;
    
    private float minTimeToSpawnPlayerBrick = 0f;
    private float maxTimeToSpawnPlayerBrick = 0f;
    private int maxSimultaneousPlayerBricks = 0;
    private float timeToSpawnPlayerBrick = 0f;
    private float spawnEnemyPlayerTimer = 0f;

    [SerializeField] private BrickTypesSO brickTypesSO;

    private CombatBarUI combatBarUI;

    private ILevelData levelSO;
    [SerializeField] private EnemyStats EnemyStats;

    [SerializeField] private PlayerStatsSO inCombatPlayerStatsSo;

    //EnemyBrickStats
    private float maxRange;
    private float randomNumber;
    private float rangeNumberToSpawn;

    private void Awake()
    {
        combatBarUI = FindAnyObjectByType<CombatBarUI>();
    }

    private void OnEnable()
    {
        LevelSelectorUI.onSelectedLevelToPlay += SetupLevel;
    }

    private void OnDisable()
    {
        LevelSelectorUI.onSelectedLevelToPlay -= SetupLevel;
    }

    void Update()
    {
        if (inCombat == true)
        {
            combatBarUI.MovePointer();

            if (canSpawnBricks == true)
            {
                SpawnEnemyBrick();

                SpawnPlayerBrick();
            }
        }
    }
    
    private void SpawnEnemyBrick()
    {
        spawnEnemyBrickTimer += Time.deltaTime;
        if (spawnEnemyBrickTimer >= timeToSpawnEnemyBrick)
        {
            BrickTypeEnum brickTypeToSpawn = levelSO.GetEnemy(EnemyStats.currentEnemy).GetRandomEnemyBrick();
            if (brickTypeToSpawn != BrickTypeEnum.Redbrick && combatBarUI.GetBrickTypeCountInBar(brickTypeToSpawn) >= 2)
            {
                brickTypeToSpawn = BrickTypeEnum.Redbrick;
            }
            combatBarUI.InitializeBrick(brickTypesSO.GetPool(brickTypeToSpawn).Pool.Get());
            timeToSpawnEnemyBrick = Random.Range(minTimeToSpawnEnemyBrick, maxTimeToSpawnEnemyBrick);
            spawnEnemyBrickTimer = 0f;
        }
    }
    
    private void SpawnPlayerBrick()
    {
        spawnEnemyPlayerTimer += Time.deltaTime;
        if (spawnEnemyPlayerTimer >= timeToSpawnPlayerBrick && combatBarUI.GetPlayerBricksInBar() < maxSimultaneousPlayerBricks)
        {
            BrickTypeEnum brickTypeToSpawn = inCombatPlayerStatsSo.GetRandomPlayerBrick();
            combatBarUI.InitializeBrick(brickTypesSO.GetPool(brickTypeToSpawn).Pool.Get());
            timeToSpawnPlayerBrick = Random.Range(minTimeToSpawnPlayerBrick, maxTimeToSpawnPlayerBrick);
            spawnEnemyPlayerTimer = 0f;
        }
    }
    
    private void SetupLevel(ILevelData level)
    {
        this.levelSO = level;
        CreateRandomTimeToSpawnBrick();
    }

    public void InCombat()
    {
        if (EnemyStats.currentHealth > 0f)
        {
            inCombat = true;
            combatBarUI.InCombat();
            combatBarUI.RestartPointerVelocity();
            CreateRandomTimeToSpawnBrick();
        }
    }

    public void OutOfCombat()
    {
        inCombat = false;
        combatBarUI.OutOfCombat();
        combatBarUI.RestartPointerPosition();
        combatBarUI.DeleteAllBricks();
    }

    public void PauseGameForComboAttack()
    {
        inCombat = false;
        combatBarUI.OutOfCombat();
        combatBarUI.RestartPointerPosition();
    }

    private void CreateRandomTimeToSpawnBrick()
    {
        minTimeToSpawnEnemyBrick = levelSO.GetEnemy(EnemyStats.currentEnemy).MinTimeToSpawnBrick;
        maxTimeToSpawnEnemyBrick = levelSO.GetEnemy(EnemyStats.currentEnemy).MaxTimeToSpawnBrick;

        minTimeToSpawnPlayerBrick = inCombatPlayerStatsSo.MinTimeToSpawnPlayerBrick;
        maxTimeToSpawnPlayerBrick = inCombatPlayerStatsSo.MaxTimeToSpawnPlayerBrick;
        maxSimultaneousPlayerBricks = inCombatPlayerStatsSo.MaxSimultaneousPlayerBricks;
        
        timeToSpawnEnemyBrick = Random.Range(minTimeToSpawnEnemyBrick, maxTimeToSpawnEnemyBrick);
        spawnEnemyBrickTimer = 0f;
        timeToSpawnPlayerBrick = Random.Range(minTimeToSpawnPlayerBrick, maxTimeToSpawnPlayerBrick);
        spawnEnemyPlayerTimer = 0f;
    }

    public void CanSpawnBrick()
    {
        canSpawnBricks = true;
    }

    public void CannotSpawnBrick()
    {
        canSpawnBricks = false;
    }

    public void SpawnYellowPlayerBrick()
    {
        SpawnBrick(brickTypesSO.GetPool(BrickTypeEnum.YellowBrick).Pool.Get());
    }
    
    public void SpawnYellowPlayerBrick(float initialPosition)
    {
        SpawnBrick(brickTypesSO.GetPool(BrickTypeEnum.YellowBrick).Pool.Get(), initialPosition);
    }
    
    public void SpawnGreenPlayerBrick()
    {
        SpawnBrick(brickTypesSO.GetPool(BrickTypeEnum.Greenbrick).Pool.Get());
    }
    
    public void SpawnRedPlayerBrick()
    {
        SpawnBrick(brickTypesSO.GetPool(BrickTypeEnum.Redbrick).Pool.Get());
    }

    private void SpawnBrick(Brick brickToSpawn)
    {
        brickToSpawn.SetTimeToAutoDelete(0f);
        combatBarUI.InitializeBrick(brickToSpawn);
    }
    
    private void SpawnBrick(Brick brickToSpawn, float initialPosition)
    {
        brickToSpawn.SetTimeToAutoDelete(0f);
        combatBarUI.InitializeBrick(brickToSpawn, initialPosition);
    }
}
