using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRender : MonoBehaviour
{
    public Sprite highlightSpriteHappy;

    private Tile _highlightedTile;
    private Tile _originalTile;
    private Vector3Int _originalPosition;
    private Tilemap _tilemap;
    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _tilemap = gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // HighlightTile();
        }
        else
        {
            // UnhighlightTile();
        }
    }

    private void HighlightTile()
    {
        Vector3Int mousePosition = GetMousePosition();
        Tile tile = _tilemap.GetTile<Tile>(mousePosition);
        if (tile == _highlightedTile)
        {
            return;
        }

        if (_originalPosition != GetMousePosition())
        {
            UnhighlightTile();
        }

        _originalTile = tile;
        _originalPosition = mousePosition;
        _highlightedTile = ScriptableObject.CreateInstance<Tile>();
        _highlightedTile.sprite = highlightSpriteHappy;
        _tilemap.SetTile(mousePosition, _highlightedTile);
    }

    private void UnhighlightTile()
    {
        if (!_originalTile)
        {
            return;
        }

        _tilemap.SetTile(_originalPosition, _originalTile);
        _highlightedTile = null;
        _originalTile = null;
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return _tilemap.WorldToCell(mouseWorldPos);
    }
}