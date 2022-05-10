using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Canvas;
    private GameObject _zoomCard;
    public Color Red;
    public Color Blue;

    GameManager _gameManager;
    //

    //

    private void OnDestroy()
    {
        Destroy(_zoomCard);
    }

    public void Awake()
    {
        Red = new Color(1, 0, 0, 1);
        Blue = new Color(0, 0, 1, 1);

        Canvas = GameObject.Find("Main Canvas");
        gameObject.GetComponent<Outline>().enabled = false;

        //Assign gamemanager
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        bool isSelectedCard = gameObject.GetComponent<DragDrop>().IsSelected;

        if (isSelectedCard == true)
        {
            gameObject.GetComponent<Outline>().enabled = true;
            gameObject.GetComponent<Outline>().effectColor = Red;
        }
        else
        {
            gameObject.GetComponent<Outline>().effectColor = Blue;
        }

        if (_zoomCard)
        {
            //get zoom card cardstats component and set the health, attack and mana to this cards stats
            _zoomCard.GetComponent<CardStats>().Health = GetComponent<CardStats>().Health;
            _zoomCard.GetComponent<CardStats>().Attack = GetComponent<CardStats>().Attack;
            _zoomCard.GetComponent<CardStats>().ManaCost = GetComponent<CardStats>().ManaCost;
        }
    }

    public void HoverOver()
    {
        //gameObject.GetComponent<Outline>().enabled = true;

        //Vector2 cardPosition = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), Canvas.GetComponent<Canvas>()).position;
        //cardPosition = GetComponent<RectTransform>().anchoredPosition;

        //_zoomCard = Instantiate(gameObject, cardPosition, Quaternion.identity);
        //RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        //rect.anchorMax = new Vector2(0, 1);
        //rect.anchorMin = new Vector2(0, 1);
        //rect.pivot = new Vector2(0, 1);

        //_zoomCard.GetComponent<Image>().raycastTarget = false;
        //_zoomCard.transform.SetParent(Canvas.transform, false);
        //_zoomCard.layer = LayerMask.NameToLayer("Zoom"); // puts the new zoomed card to a seperate layer
    }

    public void HoverExit()
    {
        //gameObject.GetComponent<Outline>().enabled = false;
        //Destroy(_zoomCard);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // if gameobject is not in opponent handlist
        if (_gameManager.Opponent.GetComponent<PlayerStats>().HandCards.Contains(gameObject))
        {
            return;
        }

        gameObject.GetComponent<Outline>().enabled = true; // enable highlight 

        Vector2 cardPosition = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), Canvas.GetComponent<Canvas>()).position;
       // Vector2 cardPosition = GetComponent<RectTransform>().anchoredPosition;

        _zoomCard = Instantiate(gameObject, cardPosition, Quaternion.identity);
        _zoomCard.GetComponent<BoxCollider2D>().enabled = false;
        

        RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0, 0);

        _zoomCard.GetComponent<Image>().raycastTarget = false;

        foreach (var child in _zoomCard.GetComponentsInChildren<Image>())
        {
            child.raycastTarget = false;
        }
        
        foreach (var child in _zoomCard.GetComponentsInChildren<TMP_Text>())
        {
            child.raycastTarget = false;
        }

        _zoomCard.transform.localScale = new Vector3(2, 2, 1);

        _zoomCard.transform.SetParent(Canvas.transform, false);
        _zoomCard.layer = LayerMask.NameToLayer("Ignore Raycast"); // puts the new zoomed card to a seperate layer
        _zoomCard.GetComponent<CardZoom>().enabled = false;
        // Calls AudioManager to Play requested sound effect
        AudioManager.Instance.Play("SFX_Card_Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Outline>().enabled = false;
        Destroy(_zoomCard);
        // Calls AudioManager to Stop requested sound effect
        AudioManager.Instance.Stop("SFX_Card_Hover");
    }
}