using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteController : MonoBehaviour
{
    public Sprite draggingSprite;
    public Tilemap tilemap;
    
    private Vector3 _offset;
    private bool _isDragging;
    private SpriteRenderer _spriteRenderer;
    private Sprite _stillSprite;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _stillSprite = _spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDrag()
    {
        if (!Camera.main)
        {
            return;
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 layeredMousePosition = new Vector3(
            mousePosition.x,
            mousePosition.y,
            transform.position.z
        );

        if (!_isDragging)
        {
            _offset = layeredMousePosition - mousePosition;
            _spriteRenderer.sprite = draggingSprite;
            _isDragging = true;
        }

        transform.position = mousePosition + _offset;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _spriteRenderer.sprite = _stillSprite;
        SnapToGrid();
    }

    private void SnapToGrid()
    {
        if (!Camera.main)
        {
            return;
        }
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);
        transform.position = tilemap.GetCellCenterWorld(tilePos);
    }
}
