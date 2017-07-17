using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public const int PLAYER_LIVES_FOR_LEVEL = 3;

    //public GameoverPage gameOverPage;
    public GameplayPage gameplayPage;

    public FieldController fieldController;
    public PlayerController playerController;
    public EnemiesController enemiesController;

    private int currentLevel = 0;

    private void OnPlayerLivesChanged(int lives)
    {
        gameplayPage.SetLives(lives);
    }

    private void OnLevelProgressChanged(int progress)
    {
        Debug.Log("OnLevelProgressChanged called: " + progress);
        gameplayPage.SetProgress(progress);
    }

    void Awake()
    {
        playerController.fieldController = fieldController;

        enemiesController.Init();

        fieldController.OnLevelProgressUpdated += OnLevelProgressChanged;
        playerController.OnPlayerLivesChanged += OnPlayerLivesChanged;

        NextLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        currentLevel++;

        fieldController.Reset();
        fieldController.CreateField(Vector3.zero, 50, 50);
        playerController.Init();
        playerController.lives = PLAYER_LIVES_FOR_LEVEL;
        enemiesController.SpawnEnemies(currentLevel, fieldController);
        
        OnPlayerLivesChanged(PLAYER_LIVES_FOR_LEVEL);
        OnLevelProgressChanged(15);
    }
    
    public void GameOver()
    {

    }
}
