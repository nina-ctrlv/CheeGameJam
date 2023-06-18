using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameConfiguration : MonoBehaviour
{
    public static GameConfiguration Instance { get; private set; }
    [FormerlySerializedAs("pathTiles")] public Sprite[] pathSprites;

    private HashSet<Sprite> _pathSprites;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        _pathSprites = new HashSet<Sprite>(pathSprites);
        Instance = this;
    }

    public bool IsPathTile(Tile tile)
    {
        return tile != null && _pathSprites.Contains(tile.sprite);
    }
}