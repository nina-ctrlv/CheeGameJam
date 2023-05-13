using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRender : MonoBehaviour
{
    public Sprite highlightSpriteHappy;
    public Sprite highlightSpriteSad;
    public Sprite allowedPathSprite;
    public Sprite mapDisallowedSprite;

    private Tile _highlightedTile;
    private Tile _originalTile;
    private Vector3Int _originalPosition;
    private Tilemap _tilemap;
    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _tilemap = gameObject.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {        // Left mouse click -> add path tile
        if (!_mainCamera && Camera.main)
        {
            _mainCamera = Camera.main;
        }

        if (!_tilemap && gameObject.GetComponent<Tilemap>())
        {
            _tilemap = gameObject.GetComponent<Tilemap>();
        }
        
        if (Input.GetMouseButton(0))
        {
            HighlightTile();
        }
        else
        {
            UnhighlightTile();
        }
    }

    private void HighlightTile()
    {
        Vector3Int mousePosition = GetMousePosition();
        Tile tile = _tilemap.GetTile<Tile>(mousePosition);
        if (tile  == _highlightedTile)
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

    Vector3Int GetMousePosition ()
    {
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return _tilemap.WorldToCell(mouseWorldPos);
    }
}