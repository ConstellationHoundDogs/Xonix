using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    EMPTY,
    FILLED,
    TRACK
}

public class TileController : MonoBehaviour
{
    public TileState CurrentTileState
    {
        get
        {
            return _currentTileState;
        }
    }

    public Color emptyColor;
    public Color trackColor;
    public Color filledColor;
    
    public bool indexed;
    public int x;
    public int y;

    private TileState _currentTileState;
    private MeshRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    
    public void ChangeTileState(TileState tileState)
    {
        _currentTileState = tileState;

        switch (tileState)
        {
            case TileState.EMPTY:
                _renderer.material.color = emptyColor;
                break;
            case TileState.TRACK:
                _renderer.material.color = trackColor;
                break;
            case TileState.FILLED:
                _renderer.material.color = filledColor;
                transform.Translate(0, 1, 0);
                break;
        }
    }
}
