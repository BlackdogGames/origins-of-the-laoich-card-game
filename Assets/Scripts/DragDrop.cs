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
    [SerializeField]
    private bool _isDragging = false;
    private Vector2 _startPosition;
    private bool _isOverDropZone = false;
    private GameObject _dropZone;
    private GameObject _startParent;

    private GameManager _gameManager;
    private GameObject _player;
    private GameManager _opponent;

    public GameObject AttackingCard;
    public GameObject CardToAttack;


    static List<GameObject> CardList;  
    
    //

    //
    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");

        PlayerDropZone = GameObject.Find("DropZone");
        OpponentDropZone = GameObject.Find("OpponentDropZone");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
       

        if (CardList == null)
            CardList = new List<GameObject>(2);
      

        
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

    public void ClearSelectionList()
    {
        CardList.Clear();
    }

    public void OnSelection()
    {
        CardStats _stats = GetComponent<CardStats>();
        bool _belongsToPlayer = _stats.BelongsToLocalPlayer;

        if (_isOverDropZone && !_isDragging)
        {
            switch (_gameManager.PlayersTurn)
            {
                case (true): // is the local player's turn

                    if (_dropZone.name == "DropZone" && !_isDragging) // if the card is in the player's dropzone and not being dragged currently
                    {
                        if (_belongsToPlayer == true && !CardList.Contains(gameObject)) // card belongs to the player and this specific card is not already in the list 
                        {
                            CardList.Add(gameObject); // add the card to the list 
                            Debug.Log("Player Card Selected; is in Attack Slot");
                        }

                    }
                    else if (_dropZone.name == "OpponentDropZone" && !_isDragging) // if the card is in the opponents dropzone and not being dragged currently
                    {
                        if (CardList.Count < 2) // if the list has less than two items 
                        {
                            if (_belongsToPlayer == false && CardList[0] != null) // if its an opponent card and the first slot is not null
                            {
                                CardList.Add(gameObject); // add the card to the list 
                                Debug.Log("Opponent Card Selected; is in Defendant Slot");
                            }
                        }


                    }

                    break;

                case (false): // is the opponents's turn

                    if (_dropZone.name == "OpponentDropZone" && !_isDragging) // if the card is in the player's dropzone and not being dragged currently
                    {
                        if (_belongsToPlayer == false && !CardList.Contains(gameObject)) //  if the card belongs to the opponent and is not already in the cardlist
                        {
                            CardList.Add(gameObject); // add it to the list 
                            Debug.Log("Player Card Selected; is in Attack Slot");
                        }
                    }
                    else if (_dropZone.name == "DropZone" && !_isDragging) // if the card is in the opponents dropzone and not being dragged currently
                    {
                        if (_belongsToPlayer == true && CardList[0] != null) // if the card belongs to the player and the first card slot is fulled 
                        {
                            CardList.Add(gameObject);
                            Debug.Log("Opponent Card Selected; is in Defendant Slot");
                        }


                    }

                    break;

            }

            if (CardList.Count >= 2) // if both slots are not empty //CardList[0] != null && CardList[1] != null)
            {
                _gameManager.CardAttackCard(CardList[0], CardList[1]); // run the attack with both cards in the list 
                CardList.Clear(); // clear the list 
                Debug.Log("Card attack occured, list cleared");
            }


        }
        
        Debug.Log(CardList.Count.ToString());
       // Debug.Log(CardList[0].name + " - " + CardList[1].name);

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

        if (_isOverDropZone && _gameManager.IsCardPlayable(gameObject, (_gameManager.PlayersTurn) ? _gameManager.PlayerStatsInstance : _gameManager.OppStatsInstance)) // Ternary statement: shorthand If statement. If it is the players turn use playerstats, else use oppstats
        {
            transform.SetParent(_dropZone.transform, false);    // Place card in play area

            // TODO: card interaction here, CardAttackCard(), CardAttackPlayer(), ManaDecrease()

           // _gameManager.PlayersTurn = !_gameManager.PlayersTurn; // Swap turns
        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
            transform.SetParent(_startParent.transform, false);
        }
    }
    //
}