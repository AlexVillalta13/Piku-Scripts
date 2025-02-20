using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabSpawn : MonoBehaviour
{
    // [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameEvent beginWalkEvent;
    GameObject instantiatedPlayerGameObject;
    [SerializeField] private PlayerPrefabController playerPrefabController;
    [SerializeField] private Transform[] playerSpawnPoints;
    private GameplayCamera gameplayCamera;
    bool canDoFirstWalk = false;

    private void Awake()
    {
        gameplayCamera = FindFirstObjectByType<GameplayCamera>();
        // playerPrefabController = FindObjectOfType<PlayerPrefabController>();
        playerPrefabController.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        // instantiatedPlayerGameObject = Instantiate(PlayerPrefab, transform.position, transform.rotation);
        playerPrefabController.gameObject.SetActive(true);
        
        playerPrefabController.transform.SetPositionAndRotation(playerSpawnPoints[0].position, playerSpawnPoints[0].rotation);
        playerPrefabController.ResetNextPositionToGo();
        gameplayCamera.SetTarget(playerPrefabController.transform);
    }

    private void OnDisable()
    {
        if (playerPrefabController != null)
        {
            playerPrefabController.gameObject.SetActive(false);
        }
    }

    public void CanWalk()
    {
        canDoFirstWalk = true;
    }

    public void BeginWalk()
    {
        if(canDoFirstWalk && gameObject.activeInHierarchy == true)
        {
            beginWalkEvent.Raise(new Empty(), this);
            canDoFirstWalk = false;
        }
    }

    public int GetPlayerSpawnPointsCount()
    {
        return playerSpawnPoints.Length;
    }

    public void ActivatePlayerInScene(int spawnPoint)
    {
        playerPrefabController.transform.SetPositionAndRotation(playerSpawnPoints[spawnPoint - 1].position, playerSpawnPoints[spawnPoint - 1].rotation);
        playerPrefabController.ResetNextPositionToGo();
        gameplayCamera.SetTarget(playerPrefabController.transform);
    }
}
