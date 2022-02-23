using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    //
    private bool _isDragging = false;
    private Vector2 _startPosition;
    //
    
    //
    void Update()
    {
       if (_isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    //

    //
    public void StartDrag()
    {
        _startPosition = transform.position;
        _isDragging = true;
    }
    //

    //
    public void EndDrag()
    {
        _isDragging = false;
    }
    //
}
