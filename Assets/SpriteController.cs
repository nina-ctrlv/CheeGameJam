using UnityEngine;
using Unity.Netcode;

public class SpriteController : NetworkBehaviour
{
    public Sprite draggingSprite;
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
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!_isDragging)
        {
            _offset = transform.position - mousePosition;
            _spriteRenderer.sprite = draggingSprite;
            _isDragging = true;
        }

        transform.position = mousePosition + _offset;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        _spriteRenderer.sprite = _stillSprite;
        if (!IsServer && IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            SendInstantiateMessageClientRpc();
        } else {
            Debug.Log("HELP");
            ServerInstantiateObjectServerRpc();
        }
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
        go.GetComponent<NetworkObject>().Spawn();
    }
}
