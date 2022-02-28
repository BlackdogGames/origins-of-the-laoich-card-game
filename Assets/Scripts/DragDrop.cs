using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    //
    private bool _isDragging = false;
    private Vector2 _startPosition;
    private bool _isOverDropZone = false;
    private GameObject _dropZone;
   
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isOverDropZone = true;
        _dropZone = collision.gameObject;
    }
    //

    private void OnCollisionExit(Collision collision)
    {
        _isOverDropZone = false;
        _dropZone = null;
    }

    //
    public void StartDrag()
    {
        _startPosition = transform.position; // logs the start position (where the card would spawn)
        _isDragging = true;
    }
    //

    //
    public void EndDrag()
    {
        _isDragging = false;

        if (_isOverDropZone)
        {
            transform.SetParent(_dropZone.transform, false);

        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
        }
    }
    //
}
