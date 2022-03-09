using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    //
    public GameObject Canvas;
    public GameObject PlayerDropZone;
    public GameObject OpponentDropZone;
    private bool _isDragging = false;
    private Vector2 _startPosition;
    private bool _isOverDropZone = false;
    private GameObject _dropZone;
    private GameObject _startParent;

    private GameManager _gameManager;


    public GameObject AttackingCard;
    public GameObject CardToAttack;


    List<GameObject> CardList;  // name for testing TODO change name


    //

    //
    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");

        PlayerDropZone = GameObject.Find("DropZone");
        OpponentDropZone = GameObject.Find("OpponentDropZone");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        CardList = new List<GameObject>();


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

        switch (_gameManager.PlayersTurn)
        {

            case (false): // opponents turn

                if (collision.gameObject.name == "OpponentDropZone" ) // if they are over the opponent's drop zone, drop the card in
                {
                    _isOverDropZone = true;
                    _dropZone = collision.gameObject;
                }

                break;


            case (true): // local player's turn
                if (collision.gameObject.name == "DropZone") // if they are over the players drop zone, drop the card in
                {
                    _isOverDropZone = true;
                    _dropZone = collision.gameObject;
                }

                break;


        }

    }
    //

    private void OnCollisionExit2D(Collision2D collision)
    {

        switch (_gameManager.PlayersTurn)
        {

            case (false): // opponents turn

                if (collision.gameObject.name == "OpponentDropZone") // if theyre no longer in the drop zone snap them back to opponents hand
                {
                    _isOverDropZone = false;
                    _dropZone = null;
                }

                break;


            case (true): // local player's turn
                if (collision.gameObject.name == "DropZone") // if theyre no longer in the drop zone snap them back to players hand
                {
                    _isOverDropZone = false;
                    _dropZone = null;
                }
                break;

        }
    }
    //

    public void OnSelection()
    {
        if (_isOverDropZone && !_isDragging)
        {
            if (CardList.Count < 2)
            {
                CardList.Add(gameObject);
            }
            else if (CardList.Count >= 2)
            {
                _gameManager.CardAttackCard(CardList[0], CardList[1]);
                CardList.Clear();
            }
        }
        

        Debug.Log(CardList.Count.ToString());


    }

    //
    public void StartDrag()
    {
        if (!_isOverDropZone)
        {
            _startParent = transform.parent.gameObject;
            _startPosition = transform.position; // logs the start position (where the card would spawn)
            _isDragging = true;
        }
        
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