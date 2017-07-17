using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int> OnPlayerLivesChanged;

    public InputController inputController;
    public FieldController fieldController;
    public GameplayController gameplayController;

    public float speed = 60;
    public int lives = 3;
    
    private bool _isMoving;
    private int _directionX;
    private int _directionY;
    private TileController _destinationTile;
    private TileController _currentTile;
    private Vector3 _tempVector;
    private Vector3 _respawnPosition = new Vector3(0, 2, 0);

    void Awake()
    {
        inputController.OnSwipeUp += OnSwipeUp;
        inputController.OnSwipeDown += OnSwipeDown;
        inputController.OnSwipeLeft += OnSwipeLeft;
        inputController.OnSwipeRight += OnSwipeRight;
    }

    private void ChooseNextTile(int y, int x)
    {
        _destinationTile = fieldController.GetTile(_currentTile.y + y, _currentTile.x + x);
    }
    
    private void CheckTile()
    {
        if (_currentTile.CurrentTileState == TileState.TRACK && _destinationTile.CurrentTileState == TileState.FILLED)
        {
            _isMoving = false;

            fieldController.FillTrack();
            fieldController.FillSmallerArea();
        }

        if (_destinationTile.CurrentTileState == TileState.TRACK)
        {
            Die();
        }
        
        _currentTile = _destinationTile;

        if (_currentTile.CurrentTileState == TileState.EMPTY)
        {
            _currentTile.ChangeTileState(TileState.TRACK);
        }

        ChooseNextTile(_directionY, _directionX);
    }

    void OnSwipeUp()
    {
        _directionX = 0;
        _directionY = 1;
        _isMoving = true;
    }

    void OnSwipeDown()
    {
        _directionX = 0;
        _directionY = -1;
        _isMoving = true;
    }

    void OnSwipeLeft()
    {
        _directionX = -1;
        _directionY = 0;
        _isMoving = true;
    }

    void OnSwipeRight()
    {
        _directionX = 1;
        _directionY = 0;
        _isMoving = true;
    }

    void Update()
    {
        //TODO: input separate input controller
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _directionX = 0;
            _directionY = 1;
            _isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _directionX = 0;
            _directionY = -1;
            _isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _directionX = -1;
            _directionY = 0;
            _isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _directionX = 1;
            _directionY = 0;
            _isMoving = true;
        }

        if (_destinationTile == null)
        {
            _isMoving = false;
            ChooseNextTile(_directionY, _directionX);
        }
        
        //TODO: ugly
        if (_isMoving)
        {
            float step = speed * Time.deltaTime;
            _tempVector = Vector3.MoveTowards(transform.position, _destinationTile.gameObject.transform.position, step);
            _tempVector.y = 2f;
            
            transform.position = _tempVector;

            float distX = Mathf.Abs(transform.position.x - _destinationTile.gameObject.transform.position.x);
            float distZ = Mathf.Abs(transform.position.z - _destinationTile.gameObject.transform.position.z);
            
            if ( distX < 0.2f && distZ < 0.2f)
            {
                CheckTile();
            }
        }
    }
    
    public void Die()
    {
        transform.position = _respawnPosition;
        fieldController.EmptyTrack();
        Init();

        lives--;

        if (OnPlayerLivesChanged != null)
        {
            OnPlayerLivesChanged(lives);
        }
        
        if (lives <= 0)
        {
            gameplayController.GameOver();
        }
    }

    public void Init()
    {
        gameObject.transform.position = new Vector3(0, 2, 0);
        _destinationTile = fieldController.GetTile(0, 0);
        _currentTile = fieldController.GetTile(0, 0);
        _isMoving = true;

        if (OnPlayerLivesChanged != null)
        {
            OnPlayerLivesChanged(lives);
        }
    }
}
