using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropForDB : MonoBehaviour
{
    //
    public GameObject Canvas;
    public GameObject PlayerDropZone;
    public GameObject DeckDropZone;

    [SerializeField]
    private bool _isDragging = false;
    private Vector2 _startPosition;
    private bool _isSelectZone = false;
    private bool _isDefaultZone = false;
    private GameObject _selectionGrid;
    private GameObject _deckGrid;
    private GameObject _startParent;

    public GameObject CardPrefab;

    private GameObject _player;

    public bool isDupe;

    static List<GameObject> CardList;

    private DeckManager _deckManager;

    //

    //
    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");

        PlayerDropZone = GameObject.Find("Selection Grid");
        DeckDropZone = GameObject.Find("Card Grid");

       _deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();

        _player = GameObject.Find("Player");

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
            // Calls AudioManager to Play requested sound effect.
            AudioManager.Instance.Play("SFX_Card_Placement");
        }

    }
    //

    //
    private void OnCollisionEnter2D(Collision2D collision)
    {

    
         if (collision.gameObject.name == "Selection Grid") // if they are over the players drop zone, drop the card in
         {
            _isSelectZone = true;
            _selectionGrid = collision.gameObject;

         }

        if (collision.gameObject.name == "Card Grid") // if they are over the players drop zone, drop the card in
        {
            _isDefaultZone = true;
            _deckGrid = collision.gameObject;

        }



    }
    //

    private void OnCollisionExit2D(Collision2D collision)
    {

        
        if (collision.gameObject.name == "Selection Grid") // if theyre no longer in the selection grid snap them back to the deck builder
         {
            _isSelectZone = false;
             _selectionGrid = null;
         }

               
    }
    //

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

        if (_isSelectZone && !isDupe) //if its over the selection szone and is not a duplicared card (is from the deck building area)
        {
            GameObject selectedCard = Instantiate(CardPrefab, transform); // create a new card
            selectedCard.name = "duped card";
            selectedCard.GetComponent<CardStats>().CardAsset = gameObject.GetComponent<CardStats>().CardAsset; // give the card the same assets
           // selectedCard.GetComponent<CardStats>().IsDuplicate = true; // set it to be a duplicate card
            selectedCard.GetComponent<DragDropForDB>().isDupe = true;
            selectedCard.transform.SetParent(_selectionGrid.transform, false);    // Place duped card in play area
            
            CardList.Add(selectedCard);

            // put the original card back in it's place
            transform.position = _startPosition;
            transform.SetParent(_deckGrid.transform, false);

        }
        else if (_isDefaultZone) // if tis over the deck building zoen
        {
           // bool dupe = gameObject.GetComponent<CardStats>().IsDuplicate;

            if (isDupe == true)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.SetParent(_deckGrid.transform, false);
            }

            
        }
        else
        {
            transform.position = _startPosition; // if the card isnt over the dropzone snap it back to the start pos
            transform.SetParent(_startParent.transform, false);
        }
    }
    //
}
