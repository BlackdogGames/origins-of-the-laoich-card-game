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
    private GameObject _droppingGridZone;
    private GameObject _startParent;

    private GameManager _gameManager;
    private GameObject _player;
    private GameObject _opponent;

    private GameObject _attackZone;

    public GameObject AttackingCard;
    public GameObject CardToAttack;

    public GameObject Pointer;

    static bool _attackCardSelected = false;
    static bool _defendCardSelected = false;
    public bool IsSelected;

    static List<GameObject> _cardList;  
    
    //

    //
    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");

        PlayerDropZone = GameObject.Find("DropZone");
        OpponentDropZone = GameObject.Find("OpponentDropZone");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
       

        if (_cardList == null)
            _cardList = new List<GameObject>(2);

        _player = GameObject.Find("Player");
        _opponent = GameObject.Find("Opponent");

        Pointer = GameObject.Find("Arrow Head");

        IsSelected = false;
    }
    //

    //
    void Update()
    {
       if (_isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
            // Calls AudioManager to Play requested sound effect.
            AudioManager.Instance.Play("SFX_Card_Placement");
        }

       if (_attackCardSelected)
        {
            Pointer.GetComponent<Cursor>().SelectTarget(Input.mousePosition, _attackCardSelected, _cardList[0]);
        }
        

    }
    //

    //
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CardStats stats = GetComponent<CardStats>();

        switch (_gameManager.PlayersTurn)
        {

            case (false): // opponents turn

                if (collision.gameObject.name == "OpponentDropZone") // if they are over the opponent's drop zone, drop the card in
                {
                    _dropZone = collision.gameObject;
                }

                if (collision.gameObject.tag == "Dropping Zone" && collision.gameObject.GetComponent<DroppingZone>().isEnemyZone == true) // if they are over the opponent's drop zone, drop the card in
                {
                    if (collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed == false)
                    {
                        collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed = true;
                        _isOverDropZone = true;
                        _droppingGridZone = collision.gameObject;

                    _opponent.GetComponent<PlayerStats>().HandCards.Remove(gameObject);
                    _opponent.GetComponent<PlayerStats>().FieldCards.Add(gameObject);
                    GetComponent<CardStats>().FirstTurnPlayed = true;
                    }
                }

                if (collision.gameObject.tag == "AttackZone" && _isOverDropZone == true )
                {
                    _attackZone = collision.gameObject;
                    stats.ZoneID = _attackZone.GetComponent<CardAttackZone>().ZoneID;
                    Debug.Log("oppponent Card in attack zone: " + stats.ZoneID);
                }



                break;


            case (true): // local player's turn

                if (collision.gameObject.name == "DropZone") // if they are over the players drop zone, drop the card in
                {
                   
                    _dropZone = collision.gameObject;
                    

                    _player.GetComponent<PlayerStats>().HandCards.Remove(gameObject);
                    _player.GetComponent<PlayerStats>().FieldCards.Add(gameObject);
                    GetComponent<CardStats>().FirstTurnPlayed = true;
                }

                if (collision.gameObject.tag == "Dropping Zone" && collision.gameObject.GetComponent<DroppingZone>().isPlayerZone == true) // if they are over the players drop zone, drop the card in
                {
                    if (collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed == false)
                    {
                       
                        _isOverDropZone = true;
                        _droppingGridZone = collision.gameObject;

                        GetComponent<CardStats>().FirstTurnPlayed = true;
                        collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed = true;

                    }
                }

                if (collision.gameObject.tag == "AttackZone" && _isOverDropZone == true )
                {
                    _attackZone = collision.gameObject;
                    stats.ZoneID = _attackZone.GetComponent<CardAttackZone>().ZoneID;
                    Debug.Log("player Card in attack zone: " + stats.ZoneID);

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

                if (collision.gameObject.tag == "Dropping Zone") // if theyre no longer in the drop zone snap them back to opponents hand
                {
                    if (collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed == true)
                    { collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed = false; }
                    _isOverDropZone = false;
                    _dropZone = null;
                    _droppingGridZone = null;
                }

                break;


            case (true): // local player's turn
                if (collision.gameObject.tag == "Dropping Zone") // if theyre no longer in the drop zone snap them back to players hand
                {
                    if (collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed == true)
                    { collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed = false; }
                    _isOverDropZone = false;
                    _dropZone = null;
                    _droppingGridZone = null;
                }
                break;

        }
    } 
    //

    public void ClearSelectionList()
    {
        _attackCardSelected = false;
        _defendCardSelected = false;
        IsSelected = false;

        _cardList.Clear();

    }


    // when a card is selected, run an attack
    public void OnSelection()
    {
        CardStats stats = GetComponent<CardStats>();
        bool belongsToPlayer = stats.BelongsToLocalPlayer;


        if (_isOverDropZone && !_isDragging)
        {
            switch (_gameManager.PlayersTurn)
            {
                case (true): // is the local player's turn
                    
                    if (_dropZone.name == "DropZone" && !_isDragging) // if the card is in the player's dropzone and not being dragged currently
                    {
                        if (!_attackCardSelected)
                        {
                            if (belongsToPlayer == true && !_cardList.Contains(gameObject)) // card belongs to the player and this specific card is not already in the list 
                            {
                                _attackCardSelected = true;
                                IsSelected = true;
                                _cardList.Add(gameObject); // add the card to the list 
                                Debug.Log("Player Card Selected; is in Attack Slot " + _attackCardSelected);

                            }
                        }
                        else if (_attackCardSelected && _cardList.Contains(gameObject)) // deselect/cancel the card
                        {
                            _cardList.Remove(gameObject); // remove the card from the selection
                            _attackCardSelected = false;
                            IsSelected = false;
                            Debug.Log("Card removed ");
                        }

                    }
                    else if (_dropZone.name == "OpponentDropZone" && !_isDragging) // if the card is in the opponents dropzone and not being dragged currently
                    {
                        if (_cardList.Count < 2) // if the list has less than two items 
                        {
                            if (!_defendCardSelected)
                            {
                                if (belongsToPlayer == false && _cardList[0] != null) // if its an opponent card and the first slot is not null
                                {
                                    _defendCardSelected = true;
                                    IsSelected = false;
                                    _cardList.Add(gameObject); // add the card to the list 
                                    Debug.Log("Opponent Card Selected; is in Defendant Slot");

                                }
                            } 
                           
                        }

                    }

                    break;

                case (false): // is the opponents's turn

                    if (_dropZone.name == "OpponentDropZone" && !_isDragging) // if the card is in the player's dropzone and not being dragged currently
                    {
                        if (belongsToPlayer == false && !_cardList.Contains(gameObject)) //  if the card belongs to the opponent and is not already in the cardlist
                        {
                            if (!_attackCardSelected)
                            {
                                _attackCardSelected = true;
                                IsSelected = true;
                                _cardList.Add(gameObject); // add it to the list 
                                Debug.Log("Player Card Selected; is in Attack Slot");
                            } 
                        }
                        else if (_attackCardSelected && _cardList.Contains(gameObject)) // deselect/cancel the card
                        {
                            _cardList.Remove(gameObject); // remove the card from the selection
                            _attackCardSelected = false;
                            IsSelected = false;
                            Debug.Log("Card removed ");
                        }
                    }
                    else if (_dropZone.name == "DropZone" && !_isDragging) // if the card is in the opponents dropzone and not being dragged currently
                    {
                        if (belongsToPlayer == true && _cardList[0] != null) // if the card belongs to the player and the first card slot is filled 
                        {
                            if (!_defendCardSelected)
                            {
                                _defendCardSelected = true;
                                IsSelected = true;
                                _cardList.Add(gameObject);
                                Debug.Log("Opponent Card Selected; is in Defendant Slot");
                            }
                        }
                       


                    }

                    break;

            }

            if (_cardList.Count >= 2) // if both slots are not empty //CardList[0] != null && CardList[1] != null)
            {
                int _firstZone = _cardList[0].GetComponent<CardStats>().ZoneID; // get the first card zone id
                int _secondZone = _cardList[1].GetComponent<CardStats>().ZoneID; // get the second card zone id

                if(_firstZone == _secondZone) // if the zones match, proceed with the attack
                {
                    if ((belongsToPlayer && !_player.GetComponent<PlayerStats>().IsFirstTurn) || (!belongsToPlayer && !_opponent.GetComponent<PlayerStats>().IsFirstTurn) && !_cardList[0].GetComponent<CardStats>().FirstTurnPlayed)
                    {
                        _gameManager.CardAttackCard(_cardList[0], _cardList[1]); // run the attack with both cards in the list 
                                                                               // clear flags //
                        _attackCardSelected = false;
                        _defendCardSelected = false;
                        IsSelected = false;
                        //
                        _cardList.Clear(); // clear the list 
                        Debug.Log("Card attack occured, list cleared");
                    }
                    else
                    {
                        // clear flags //
                        _attackCardSelected = false;
                        _defendCardSelected = false;
                        IsSelected = false;
                        //
                        _cardList.Clear();
                        Debug.Log("First turn detected, no action conducted, list cleared");
                    }
                }
                else
                {
                    // clear flags //
                    _attackCardSelected = false;
                    _defendCardSelected = false;
                    IsSelected = false;
                    //
                    _cardList.Clear();
                    Debug.Log("Zones are not compatible; list cleared, no action conducted");
                }

                
            }


        }
        
        Debug.Log(_cardList.Count.ToString());
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

        if (_isOverDropZone && _gameManager.IsCardPlayable(gameObject, (_gameManager.PlayersTurn) ? _gameManager.PlayerStatsInstance : _gameManager.OppStatsInstance)) // Ternary statement: shorthand If statement. if the player has enough mana, play the card and decrease player mana
        {
            transform.SetParent(_droppingGridZone.transform, false);    // Place card in play area

            _gameManager.ManaDecrease(gameObject, (_gameManager.PlayersTurn) ? _gameManager.PlayerStatsInstance : _gameManager.OppStatsInstance); // run the mana decrease function
        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
            transform.SetParent(_startParent.transform, false);
        }
    }
    //
}