using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    //
    public GameObject Canvas;
    private bool _isDragging = false;
    private Vector2 _startPosition;
    private bool _isOverDropZone = false;
    private GameObject _dropZone;
    private GameObject _startParent;

    private GameManager _gameManager;
    //

    //
    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //

    //
    void Update()
    {
       if (_isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isOverDropZone = false;
        _dropZone = null;
    }

    //
    public void StartDrag()
    {
        _startParent = transform.parent.gameObject;
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
            transform.SetParent(_dropZone.transform, false);    // Place card in play area

            // TODO: card interaction here, CardAttackCard(), CardAttackPlayer(), ManaDecrease()

            _gameManager.PlayersTurn = !_gameManager.PlayersTurn; // Swap turns
        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
            transform.SetParent(_startParent.transform, false);
        }
    }
    //
}
