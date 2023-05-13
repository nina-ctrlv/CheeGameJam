using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Vector3 _offset;
    private bool _isDragging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
            _isDragging = true;
        }

        transform.position = mousePosition + _offset;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
