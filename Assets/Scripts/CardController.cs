using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class CardController : NetworkBehaviour
{
    public Tilemap tilemap;
    public Card card;

    private Vector3 _originalPosition;
    private bool _isDragging;
    private SpriteRenderer _spriteRenderer;
    private Sprite _stillSprite;
    private Sprite _draggingSprite;
    private GameConfiguration _gameConfiguration;
    [FormerlySerializedAs("toCreate")] public GameObject _toCreate;

    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _stillSprite = _spriteRenderer.sprite;
        _draggingSprite = card.draggingSprite;
        _toCreate = card.itemToSpawn;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDrag()
    {
        SetTransformVisibility(false);
        if (!Camera.main)
        {
            return;
        }

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = transform.position.z;

        if (!_isDragging)
        {
            _spriteRenderer.sprite = _draggingSprite;
            _isDragging = true;
        }

        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _spriteRenderer.sprite = _stillSprite;

        Vector3Int? position = GetMousePositionTile();

        if (position == null || !CanDropTile(position.Value))
        {
            SetTransformVisibility(true);
            transform.position = _originalPosition;
            _isDragging = false;
            return;
        }

        SnapToGrid(position.Value);

        if (!IsServer &&
            IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            SendInstantiateMessageClientRpc();
        }
        else
        {
            Debug.Log("HELP");
            ServerInstantiateObjectServerRpc();
        }

        // Remove from hand and destroy object when placed
        GameObject.Find("Hand").GetComponent<PlayerHand>().hand.Remove(this.GetComponent<CardDisplay>().card);
        Destroy(gameObject);
    }

    private bool CanDropTile(Vector3Int position)
    {
        var tile = tilemap.GetTile<Tile>(position);
        var isPathTile = GameConfiguration.Instance.IsPathTile(tile);

        return card.type == CardType.Food ? isPathTile : !isPathTile;
    }

    private void SetTransformVisibility(bool isVisible)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isVisible);
        }
    }

    private void SnapToGrid(Vector3Int position)
    {
        transform.position = tilemap.GetCellCenterWorld(position);
    }

    private Vector3Int? GetMousePositionTile()
    {
        if (!Camera.main)
        {
            return null;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return tilemap.WorldToCell(mouseWorldPos);
    }

    [ClientRpc]
    void SendInstantiateMessageClientRpc()
    {
        Debug.Log($"Client sending instantiate message");
        if (IsOwner)
        {
            ServerInstantiateObjectServerRpc();
        }
    }

    [ServerRpc]
    void ServerInstantiateObjectServerRpc()
    {
        Debug.Log($"Server instantiating object");
        GameObject go = Instantiate(this._toCreate, transform.position, Quaternion.identity);
        go.GetComponent<Spawner>().spawnData = this.GetComponent<CardDisplay>().card;
        go.GetComponent<NetworkObject>().Spawn();
    }
}