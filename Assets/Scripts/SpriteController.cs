using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Netcode;

public class SpriteController : NetworkBehaviour
{
    public Sprite draggingSprite;
    public Tilemap tilemap;
    public GameObject toCreate;

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

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = transform.position.z;

        if (!_isDragging)
        {
            _spriteRenderer.sprite = draggingSprite;
            _isDragging = true;
        }

        for(var i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _spriteRenderer.sprite = _stillSprite;
        SnapToGrid();
        if (!IsServer && IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
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
        GameObject go = Instantiate(this.toCreate, transform.position, Quaternion.identity);
        go.GetComponent<Spawner>().spawnData = this.GetComponent<CardDisplay>().card;
        go.GetComponent<NetworkObject>().Spawn();
    }
}
