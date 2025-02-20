using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActivator : MonoBehaviour
{
    // [SerializeField] private int enemyToSpawn = 5;
    
    [SerializeField] GameObject mainMenuScene;
    [SerializeField] SceneElementsHolder alpineWoodsScene;
    [SerializeField] SceneElementsHolder forestScene;

    [SerializeField] private SceneElementsHolder[] scenesToQueue;
    private Queue<SceneElementsHolder> scenesQueue;
    private SceneElementsHolder activatedScene;

    ILevelData currentLevelToLoad;

    private void OnEnable()
    {
        LevelSelectorUI.onSelectedLevelToPlay += LoadLevel;
    }

    private void OnDisable()
    {
        LevelSelectorUI.onSelectedLevelToPlay -= LoadLevel;
    }

    private void Start()
    {
        EnableMainMenu();
    }

    public void DisableAllScenes()
    {
        mainMenuScene.SetActive(false);
        DisableAllGameplayScenes();
        alpineWoodsScene.gameObject.SetActive(false);
        forestScene.gameObject.SetActive(false);
    }

    private void DisableAllGameplayScenes()
    {
        foreach (SceneElementsHolder scene in scenesToQueue)
        {
            scene.gameObject.SetActive(false);
        }
    }

    public void EnableMainMenu()
    {
        DisableAllScenes();
        mainMenuScene.SetActive(true);
    }

    private void DisableMainMenu()
    {
        mainMenuScene.SetActive(false);
    }

    public void BeginGameplay()
    {
        DisableMainMenu();
        SetQueue();
        ActivateFirstScene();
        // EnableNextSceneFromQueue();
    }

    private void ActivateFirstScene()
    {
        bool continueLoop = true;
        // int enemyCount = enemyToSpawn;
        int enemyCount = currentLevelToLoad.GetCurrentEnemy();
        while (continueLoop)
        {
            EnableNextSceneFromQueue();
            enemyCount -= activatedScene.GetPlayerSpawnPointsCount();
            if(enemyCount <= 0)
            {
                continueLoop = false;
                activatedScene.SetPlayerSpawnPosition(enemyCount + activatedScene.GetPlayerSpawnPointsCount());
            }
        }
    }

    private void SetQueue()
    {
        scenesQueue = new Queue<SceneElementsHolder>(scenesToQueue);
    }

    public void EnableNextSceneFromQueue()
    {
        if (activatedScene != null && activatedScene.gameObject.activeInHierarchy == true)
        {
            activatedScene.gameObject.SetActive(false);
        }

        activatedScene = scenesQueue.Dequeue();
        scenesQueue.Enqueue(activatedScene);
        EnableScene(activatedScene);
    }

    public void ActivateSelectedSceneEnvironment()
    {
        DisableAllScenes();
        switch (currentLevelToLoad.GetEnvironment())
        {
            case SceneEnum.AlpineWoods:
                EnableScene(alpineWoodsScene);
                break;
            case SceneEnum.Forest:
                EnableScene(forestScene);
                break;
        }
    }

    private void EnableScene(SceneElementsHolder scene)
    {
        scene.gameObject.SetActive(true);
        scene.SetupLevel(currentLevelToLoad.GetTotalEnemiesCount());
    }

    private void LoadLevel(ILevelData level)
    {
        currentLevelToLoad = level;
    }
}