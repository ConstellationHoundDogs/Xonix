using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public GameplayController gameplayController;
    public FieldController fieldController;
    public PlayerController playerController;

    public EnemyController emptyEnemyPrefab;
    public EnemyController filledEnemyPrefab;
    
    private List<GameObject> _fillEnemies;
    private List<GameObject> _emptyEnemies;

    public void Init()
    {
        _fillEnemies = new List<GameObject>();
        _emptyEnemies = new List<GameObject>();
    }
    
    public void SpawnEnemies(int level, FieldController fieldController)
    {
        foreach(GameObject enemy in _fillEnemies)
        {
            Destroy(enemy);
        }

        foreach (GameObject enemy in _emptyEnemies)
        {
            Destroy(enemy);
        }

        _fillEnemies.Clear();
        _emptyEnemies.Clear();

        //TODO refactor
        for (int i = 0; i < level; i++)
        {
            GameObject emptyEnemy = Instantiate(emptyEnemyPrefab.gameObject);
            EnemyController enemyController = emptyEnemy.GetComponent<EnemyController>();
            enemyController.fieldController = fieldController;
            enemyController.playerController = playerController;
            enemyController.enemiesController = this; 
            enemyController.enemyArea = TileState.EMPTY;
            enemyController.StartMovement(fieldController.GetRandomTileOfType(TileState.EMPTY));
            _emptyEnemies.Add(emptyEnemy);
        }

        for (int i = 0; i < (level / 2) + 1; i++)
        {
            GameObject filledEnemy = Instantiate(filledEnemyPrefab.gameObject);
            EnemyController enemyController = filledEnemy.GetComponent<EnemyController>();
            enemyController.fieldController = fieldController;
            enemyController.playerController = playerController;
            enemyController.enemiesController = this;
            enemyController.enemyArea = TileState.FILLED;
            enemyController.StartMovement(fieldController.GetRandomTileOfType(TileState.FILLED));
            _fillEnemies.Add(filledEnemy);
        }
    }

    public void KillEnemy(EnemyController enemy)
    {
        Destroy(enemy.gameObject);
        if (enemy.enemyArea == TileState.EMPTY)
        {
            _emptyEnemies.Remove(enemy.gameObject);

            if (_emptyEnemies.Count == 0)
            {
                gameplayController.NextLevel();
            }
        }
        else
        {
            _fillEnemies.Remove(enemy.gameObject);
        }
    }
}
