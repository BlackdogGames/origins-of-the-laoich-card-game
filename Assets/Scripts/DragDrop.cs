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

    private int _lastZone;

    public static List<GameObject> CardList;  
    
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
            Pointer.GetComponent<Cursor>().SelectTarget(Input.mousePosition, _attackCardSelected, CardList[0]);
        }
        if (!_attackCardSelected)
        {
            Pointer.GetComponent<Cursor>().Default();
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
                        _isOverDropZone = true;
                        _droppingGridZone = collision.gameObject;
                        
                        GetComponent<CardStats>().FirstTurnPlayed = true;
                    }
                }

                if (collision.gameObject.tag == "AttackZone" && _isOverDropZone == true )
                {
                    _attackZone = collision.gameObject;
                    stats.ZoneID = _attackZone.GetComponent<CardAttackZone>().ZoneID;
                    _lastZone = stats.ZoneID;
                    Debug.Log("oppponent Card in attack zone: " + stats.ZoneID);
                }



                break;


            case (true): // local player's turn

                if (collision.gameObject.name == "DropZone") // if they are over the players drop zone, drop the card in
                {
                    _dropZone = collision.gameObject;

                    GetComponent<CardStats>().FirstTurnPlayed = true;
                }

                if (collision.gameObject.tag == "Dropping Zone" && collision.gameObject.GetComponent<DroppingZone>().isPlayerZone == true) // if they are over the players drop zone, drop the card in
                {
                    if (collision.gameObject.GetComponent<DroppingZone>().IsBeingUsed == false)
                    {
                        
                        _isOverDropZone = true;
                        _droppingGridZone = collision.gameObject;

                        GetComponent<CardStats>().FirstTurnPlayed = true;

                    }
                }

                if (collision.gameObject.tag == "AttackZone" && _isOverDropZone == true )
                {
                    _attackZone = collision.gameObject;
                    stats.ZoneID = _attackZone.GetComponent<CardAttackZone>().ZoneID;
                    _lastZone = stats.ZoneID; // the last zone the card was in
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
                    _isOverDropZone = false;
                    _dropZone = null;
                    _droppingGridZone = null;
                }


                break;


            case (true): // local player's turn
                if (collision.gameObject.tag == "Dropping Zone") // if theyre no longer in the drop zone snap them back to players hand
                {
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
        

        CardList.Clear();

    }


    // when a card is selected, run an attack
    public void OnSelection()
    {
        CardStats stats = GetComponent<CardStats>();
        PlayerStats oppStats = _opponent.GetComponent<PlayerStats>();
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
                            if (belongsToPlayer == true && !CardList.Contains(gameObject)) // card belongs to the player and this specific card is not already in the list 
                            {
                                _attackCardSelected = true;
                                IsSelected = true;
                                CardList.Add(gameObject); // add the card to the list 
                                Debug.Log("Player Card Selected; is in Attack Slot " + _attackCardSelected);
                                // add the opponent card using GetCardFromZone
                                GameObject enemyCardinZone = GetCardFromZone(oppStats, stats.ZoneID);
                                if (enemyCardinZone != null)
                                {
                                    CardList.Add(enemyCardinZone);
                                    _defendCardSelected = true;
                                    IsSelected = false;
                                    Debug.Log("Opponent Card Selected; is in Defendant Slot");
                                }
                                else
                                {
                                    if (gameObject.GetComponent<CardStats>().HasAttackedOpponent == false)
                                    {
                                        gameObject.GetComponent<CardStats>().HasAttackedOpponent = true;
                                        IsSelected = false;
                                        _gameManager.CardAttackPlayer(CardList[0], _opponent);
                                        Debug.Log("No Enemy Card in Zone, attack opp directly instead");
                                    }
                                    
                                }
                               

                            }
                        }
                        else if (_attackCardSelected && CardList.Contains(gameObject)) // deselect/cancel the card
                        {
                            CardList.Remove(gameObject); // remove the card from the selection
                            _attackCardSelected = false;
                            IsSelected = false;
                            Debug.Log("Card removed ");
                        }

                    }
                    
                    break;

                

            }

            if (CardList.Count >= 2) // if both slots are not empty //CardList[0] != null && CardList[1] != null)
            {
                int _firstZone = CardList[0].GetComponent<CardStats>().ZoneID; // get the first card zone id
                int _secondZone = CardList[1].GetComponent<CardStats>().ZoneID; // get the second card zone id

                if(_firstZone == _secondZone) // if the zones match, proceed with the attack
                {
                    if ((belongsToPlayer && !_player.GetComponent<PlayerStats>().IsFirstTurn) || (!belongsToPlayer && !_opponent.GetComponent<PlayerStats>().IsFirstTurn) && !CardList[0].GetComponent<CardStats>().FirstTurnPlayed)
                    {
                        _gameManager.CardAttackCard(CardList[0], CardList[1]); // run the attack with both cards in the list 
                                                                               // clear flags //
                        _attackCardSelected = false;
                        _defendCardSelected = false;
                        //IsSelected = false;
                        CardList[0].GetComponent<DragDrop>().IsSelected = false;
                        CardList[1].GetComponent<DragDrop>().IsSelected = false;
                        //
                        CardList.Clear(); // clear the list 
                        Debug.Log("Card attack occured, list cleared");
                        Debug.Log(IsSelected);
                    }
                    else
                    {
                        // clear flags //
                        _attackCardSelected = false;
                        _defendCardSelected = false;
                        IsSelected = false;
                       
                        //
                        CardList.Clear();
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
                    CardList.Clear();
                    Debug.Log("Zones are not compatible; list cleared, no action conducted");
                }

                
            }


        }
        
        Debug.Log(CardList.Count.ToString());
       // Debug.Log(CardList[0].name + " - " + CardList[1].name);

    }

    public GameObject GetCardFromZone(PlayerStats player, int zoneID)
    {
        //get field list from player, return first object that has matching zone id
        List<GameObject> fieldList = player.GetComponent<PlayerStats>().FieldCards;

        foreach (GameObject card in fieldList)
        {
            if (card.GetComponent<CardStats>().ZoneID == zoneID)
            {
                return card;
            }
        }

        return null;
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
            PlayCardToZone(_droppingGridZone);
        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
            transform.SetParent(_startParent.transform, false);
        }
    }

    public void PlayCardToZone(GameObject zone)
    {
        _droppingGridZone = zone;

        if (!_gameManager.PlayersTurn)
        {
            _isOverDropZone = true;
            _dropZone = GameObject.Find("OpponentDropZone");
        }


        transform.SetParent(zone.transform, false); // Place card in play area
        zone.GetComponent<DroppingZone>().IsBeingUsed = true;

        //if players turn, remove card from hand list and add it to field list
        if (_gameManager.PlayersTurn)
        {
            _gameManager.PlayerStatsInstance.HandCards.Remove(gameObject);
            _gameManager.PlayerStatsInstance.FieldCards.Add(gameObject);
        }
        else
        {
            _gameManager.OppStatsInstance.HandCards.Remove(gameObject);
            _gameManager.OppStatsInstance.FieldCards.Add(gameObject);
        }

        //if card ability type is onplayed, run the ability
        if (gameObject.GetComponent<CardStats>().CardAsset.AbilityTrigger == Card.CardAbilityTrigger.OnPlayed)
        {
            gameObject.GetComponent<CardStats>().CardAsset.Ability.Invoke(_gameManager, GetComponent<CardStats>());
        }

        _gameManager.ManaDecrease(gameObject,
            (_gameManager.PlayersTurn)
                ? _gameManager.PlayerStatsInstance
                : _gameManager.OppStatsInstance); // run the mana decrease function
    }
    //
}