using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFollower : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite[] pathSprites;
    public Sprite friendlySprite;
    public Sprite enemySprite;

    private Vector3Int _currentTilePosition;
    private LinkedListNode<Vector3Int> _nextPathNode;
    private bool _complete;
    private Vector3 _movement;
    private HashSet<Sprite> _pathSprites;

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
        _pathSprites = new HashSet<Sprite>(pathSprites);
        SnapToMap();
        PlanPath();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_complete)
        {
            return;
        }
        RecalculateMovement();
        transform.position += _movement;
    }

    private void RecalculateMovement()
    {
            var distanceToDestination =
            Vector3.Distance(transform.position, tilemap.GetCellCenterWorld(_nextPathNode.Value));
        if (distanceToDestination > 0.1)
        {
            return;
        }

        _currentTilePosition = tilemap.WorldToCell(transform.position);
        _nextPathNode = _nextPathNode.Next;

        if (IsEnemyPathSegment(_currentTilePosition))
        {
            _complete = true;
            return;
        }

        if (_nextPathNode == null)
        {
            _complete = true;
            throw new Exception("Expected to have a next path node to move to!");
        }
        
        UpdateMovementVector();
    }

    private void UpdateMovementVector()
    {
        var nextWorldValue = tilemap.GetCellCenterWorld(_nextPathNode.Value);
        var differenceVector = nextWorldValue - transform.position;
        _movement = differenceVector / 25;
    }

    private void PlanPath()
    {
        var currentTilePosition = tilemap.WorldToCell(transform.position);
        var neighbors = GetNeighbors(currentTilePosition, _straightNeighbors);
        var candidates = neighbors.Select(n => GetPathPlan(currentTilePosition, n));
        var refinedCandidates = candidates.Where(IsCandidatePath).ToArray();

        if (refinedCandidates.Length != 1)
        {
            throw new Exception("Expected to have exactly one candidate path");
        }

        _nextPathNode = refinedCandidates[0].First;
        _currentTilePosition = currentTilePosition;
        UpdateMovementVector();
    }

    private bool IsCandidatePath(LinkedList<Vector3Int> path)
    {
        return IsEnemyPathSegment(path.Last.Value);
    }

    private LinkedList<Vector3Int> GetPathPlan(Vector3Int current, Vector3Int next)
    {
        var path = new LinkedList<Vector3Int>();
        path.AddFirst(next);
        ConstructPath(path, current, next);
        return path;
    }

    private void ConstructPath(LinkedList<Vector3Int> path, Vector3Int previous, Vector3Int current)
    {
        while (true)
        {
            var neighbors = GetNeighbors(current, _straightNeighbors);
            var count = 0;
            Vector3Int next = default;
            
            foreach (var neighbor in neighbors)
            {
                if (neighbor != previous)
                {
                    count++;
                    next = neighbor;
                }
            }
            
            if (count == 0)
            {
                var position = GetFinalPathSegment(current);
                path.AddLast(position);
                return;
            }

            if (count != 1)
            {
                throw new Exception("Expected to have exactly one next path");
            }
            
            path.AddLast(next);

            previous = current;
            current = next;
        }
    }

    private Vector3Int GetFinalPathSegment(Vector3Int currentPosition)
    {
        foreach (var neighbor in _straightNeighbors)
        {
            var position = currentPosition + neighbor;
            var tile = tilemap.GetTile<Tile>(position);
            if (tile && (tile.sprite == friendlySprite || tile.sprite == enemySprite))
            {
                return position;
            }
        }

        throw new Exception("Expected there to be a final path segment");
    }

    private LinkedList<Vector3Int> GetNeighbors(Vector3Int currentPosition, Vector3Int[] neighborPositions)
    {
        var neighbors = new LinkedList<Vector3Int>();
        var isFinalPathSegment = false;
        
        foreach (var neighbor in neighborPositions)
        {
            var position = currentPosition + neighbor;
            
            if (IsPathTile(position))
            {
                neighbors.AddLast(position);
            }

            if (IsFinalPathSegment(position))
            {
                isFinalPathSegment = true;
            }
        }

        if (neighbors.Count == 0 && !isFinalPathSegment)
        {
            throw new Exception("Unexpected final path segment!");
        }

        return neighbors;
    }

    private bool IsFriendlyPathSegment(Vector3Int position)
    {
        var tile = tilemap.GetTile<Tile>(position);
        return tile != null && tile.sprite == friendlySprite;
    }

    private bool IsEnemyPathSegment(Vector3Int position)
    {
        var tile = tilemap.GetTile<Tile>(position);
        return tile != null && tile.sprite == enemySprite;
    }

    private bool IsFinalPathSegment(Vector3Int position)
    {
        return IsFriendlyPathSegment(position) || IsEnemyPathSegment(position);
    }

    private void SnapToMap()
    {
        var tilePosition = tilemap.WorldToCell(transform.position);
        transform.position = IsPathTile(tilePosition)
            ? GetTileWorldPosition(tilePosition)
            : FindAdjacentTile(tilePosition);
    }

    private Vector3 FindAdjacentTile(Vector3Int tilePosition)
    {
        var neighbors = GetNeighbors(tilePosition, _straightNeighbors);
        if (neighbors.Count == 0)
        {
            neighbors = GetNeighbors(tilePosition, _diagonalNeighbors);
        }

        if (neighbors.Count == 0)
        {
            throw new Exception("Could not find a suitable path to snap to");
        }

        return GetTileWorldPosition(neighbors.First.Value);
    }

    private Vector3 GetTileWorldPosition(Vector3 tilePosition)
    {
        var worldPosition = tilemap.WorldToCell(tilePosition);
        return tilemap.GetCellCenterWorld(worldPosition);
    }

    private bool IsPathTile(Vector3Int position)
    {
        var tile = tilemap.GetTile<Tile>(position);
        return tile != null && _pathSprites.Contains(tile.sprite);
    }
}