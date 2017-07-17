using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemiesController enemiesController;
    public FieldController fieldController;
    public PlayerController playerController;
    public TileState enemyArea;
    public float speed = 25f;


    private TileController _currentTile;
    private TileController _destinationTile;
    
    private int _directionX;
    private int _directionY;
    private bool _isMoving;
    private bool _movingUp;
    private Vector3 _tempVector;

    private void ChooseNextTile(int y, int x)
    {
        _destinationTile = fieldController.GetTile(_currentTile.y + y, _currentTile.x + x);
    }
    
    private void Die()
    {
        enemiesController.KillEnemy(this);
    }

    private bool isCorrectDirectionTile(TileController tile)
    {
        if (tile == null)
        {
            return false;
        }
        
        if (tile.CurrentTileState == enemyArea)
        {
            return true;
        }
        
        return false;
    }

    private void CheckDirection()
    {
        if (isCorrectDirectionTile(fieldController.GetTile(_currentTile.y + _directionY, _currentTile.x -_directionX)))
        {
            _directionX = -_directionX;
        }
        else if (isCorrectDirectionTile(fieldController.GetTile(_currentTile.y - _directionY, _currentTile.x + _directionX)))
        {
            _directionY = -_directionY;
        }
        else if (isCorrectDirectionTile(fieldController.GetTile(_currentTile.y - _directionY, _currentTile.x - _directionX)))
        {
            _directionX = -_directionX;
            _directionY = -_directionY;
        }
        else
        {
            Die();
        }
        
        ChooseNextTile(_directionY, _directionX);
    }

    void Update()
    {
        if (_isMoving)
        {
            if (_destinationTile == null)
            {
                CheckDirection();
            }

            float distX = Mathf.Abs(playerController.transform.position.x - _destinationTile.transform.position.x);
            float distZ = Mathf.Abs(playerController.transform.position.z - _destinationTile.transform.position.z);  

            if (distX < 0.6f && distZ < 0.6f)
            {
                playerController.Die();
            }

            if (enemyArea == TileState.EMPTY && _destinationTile.CurrentTileState == TileState.TRACK)
            {
                playerController.Die();
            }

            if (_destinationTile.CurrentTileState != enemyArea)
            {
                CheckDirection();
                return;
            }
            
            float step = speed * Time.deltaTime;
            _tempVector = Vector3.MoveTowards(transform.position, _destinationTile.gameObject.transform.position, step);
            _tempVector.y = 2f;

            transform.position = _tempVector;

            distX = Mathf.Abs(transform.position.x - _destinationTile.gameObject.transform.position.x);
            distZ = Mathf.Abs(transform.position.z - _destinationTile.gameObject.transform.position.z);

            if (distX < 0.2f && distZ < 0.2f)
            {
                _currentTile = _destinationTile;
                ChooseNextTile(_directionY, _directionX);
            }
        }
    }


    public void StartMovement(TileController tile)
    {
        _currentTile = tile;

        _directionX = 1;
        _directionY = -1;

        _isMoving = true;
    }
}
