using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFollower : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite pathSprite;
    public Sprite friendlySprite;
    public Sprite enemySprite;

    private Vector3Int _currentTilePosition;

    private readonly Vector3Int[] _straightNeighbors =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right,
    };

    private readonly Vector3Int[] _diagonalNeighbors =
    {
        Vector3Int.up + Vector3Int.left,
        Vector3Int.up + Vector3Int.right,
        Vector3Int.down + Vector3Int.left,
        Vector3Int.down + Vector3Int.right,
    };

    // Start is called before the first frame update
    private void Start()
    {
        SnapToMap();
        var nextPosition = GetNextTilePosition(_currentTilePosition, _currentTilePosition);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SnapToMap()
    {
        var tilePosition = tilemap.WorldToCell(transform.position);
        transform.position = IsPathTile(tilePosition)
            ? GetTileWorldPosition(tilePosition)
            : FindAdjacentTile(tilePosition);
        _currentTilePosition = tilemap.WorldToCell(transform.position);
    }

    private Vector3 FindAdjacentTile(Vector3Int tilePosition)
    {
        foreach (var mNeighbor in _straightNeighbors)
        {
            var position = tilePosition + mNeighbor;
            if (IsPathTile(position))
            {
                return GetTileWorldPosition(position);
            }
        }
        
        foreach (var mNeighbor in _diagonalNeighbors)
        {
            var position = tilePosition + mNeighbor;
            if (IsPathTile(position))
            {
                return GetTileWorldPosition(position);
            }
        }

        throw new Exception("Could not find a suitable path to snap to");
    }

    private Vector3 GetTileWorldPosition(Vector3 tilePosition)
    {
        var worldPosition = tilemap.WorldToCell(tilePosition);
        return tilemap.GetCellCenterWorld(worldPosition);
    }

    private bool IsPathTile(Vector3Int position)
    {
        var tile = tilemap.GetTile<Tile>(position);
        return tile != null && tile.sprite == pathSprite;
    }

    private Vector3Int GetNextTilePosition(Vector3Int lastPosition, Vector3Int currentPosition)
    {
        foreach (var neighbor in _straightNeighbors)
        {
            var position = currentPosition + neighbor;
            if (IsPathTile(position) && position != lastPosition)
            {
                return position;
            }
        }

        throw new Exception("Could not determine a next path");
    }
}