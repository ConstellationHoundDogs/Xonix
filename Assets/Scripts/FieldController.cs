using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    private const int TILE_SIZE = 5;

    public Action<int> OnLevelProgressUpdated;
    

    public GameplayController gameplayController;
    public GameObject tilePrefab;

    public TileController[,] tiles;

    private int _width;
    private Vector3 _tempVector;

    private Dictionary<int, List<TileController>> _areas;

    
    private void CheckLevelEnd()
    {
        int filledCount = 0;

        foreach (TileController tile in tiles)
        {
            if (tile.CurrentTileState == TileState.FILLED)
            {
                filledCount++;
            }
        }

        int percentage = (int)(((float)filledCount / tiles.Length) * 100);

        if (OnLevelProgressUpdated != null)
        {
            OnLevelProgressUpdated(percentage);
        }

        if (percentage >= 75)
        {
            gameplayController.NextLevel();
        }
    }

    private void IndexAdjacentTiles(TileController tile, int index)
    {
        if (tile == null || tile.indexed || tile.CurrentTileState != TileState.EMPTY)
        {
            return;
        }
        
        tile.indexed = true;

        _areas[index].Add(tile);
        
        IndexAdjacentTiles(GetTile(tile.y, tile.x + 1), index);
        IndexAdjacentTiles(GetTile(tile.y + 1, tile.x), index);
        IndexAdjacentTiles(GetTile(tile.y, tile.x - 1), index);
        IndexAdjacentTiles(GetTile(tile.y - 1, tile.x), index);
    }

    public TileController GetRandomTileOfType(TileState tileState)
    {

        List<TileController> selectedTiles = new List<TileController>();

        foreach (TileController tile in tiles)
        {
            if (tile.CurrentTileState == tileState)
            {
                selectedTiles.Add(tile);
            }
        }

        return selectedTiles[UnityEngine.Random.Range(0, selectedTiles.Count)];
    }

    public TileController GetTile(int y, int x)
    {
        if (y < 0 || y >= tiles.GetLength(0))
        {
            return null;
        }

        if (x < 0 || x >= tiles.GetLength(1))
        {
            return null;
        }
        
        return tiles[y,x]; 
    }
    
    public void EmptyTrack()
    {
        foreach (TileController tile in tiles)
        {
            if (tile.CurrentTileState == TileState.TRACK)
            {
                tile.ChangeTileState(TileState.EMPTY);
            }
        }
    }

    public void FillTrack()
    {
        foreach (TileController tile in tiles)
        {
            if (tile.CurrentTileState == TileState.TRACK)
            {
                tile.ChangeTileState(TileState.FILLED);
            }
        }
    }
    
    public void FillSmallerArea()
    {
        _areas = new Dictionary<int, List<TileController>>();

        int currentAreaIndex = 0;

        foreach (TileController tile in tiles)
        {
            _areas.Add(currentAreaIndex, new List<TileController>());
            IndexAdjacentTiles(tile, currentAreaIndex);
            currentAreaIndex++;
        }
        
        int smallestArea = int.MaxValue;
        int smallestAreaIndex = 0;
        int areasCount = 0;
        foreach (KeyValuePair<int, List<TileController>> keyValuePair in _areas)
        {
            if (keyValuePair.Value.Count == 0)
            {
                continue;
            }

            areasCount++;

            if (smallestArea > keyValuePair.Value.Count)
            {
                smallestAreaIndex = keyValuePair.Key;
                smallestArea = keyValuePair.Value.Count;
            }
        }
        
        if (areasCount > 1)
        {
            foreach (TileController tile in _areas[smallestAreaIndex])
            {
                tile.ChangeTileState(TileState.FILLED);
            }
        }

        CheckLevelEnd();

        foreach (TileController tile in tiles)
        {
            tile.indexed = false;
        }
    }

    public void Reset()
    {
        if (tiles == null)
        {
            return;
        }
        
        foreach (TileController tile in tiles)
        {
            Destroy(tile.gameObject);
        }
    }

    public void CreateField(Vector3 startPosition, int height, int width)
    {
        _width = width;
        _tempVector.y = startPosition.y;

        tiles = new TileController[height,width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _tempVector.z = i + startPosition.z;
                _tempVector.x = j + startPosition.x;

                GameObject tile = Instantiate(tilePrefab, _tempVector, Quaternion.identity);
                tile.name = "Tile_" + i + "_" + j;
                TileController tileController = tile.GetComponent<TileController>();
                tileController.x = j;
                tileController.y = i;
                tile.transform.SetParent(gameObject.transform);

                tiles[i,j] = tile.GetComponent<TileController>();
                
                if (j < 2 || i < 2 || i > height - 3 || j > width - 3)
                {
                    tileController.ChangeTileState(TileState.FILLED);
                }
                else
                {
                    tileController.ChangeTileState(TileState.EMPTY);
                }
            }
        }
    }

    public void DestoryField()
    {
        foreach (TileController tile in tiles)
        {
            Destroy(tile.gameObject);
        }
    }
}
