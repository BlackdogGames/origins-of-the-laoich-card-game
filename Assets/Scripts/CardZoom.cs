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
    //

    //

    private void OnDestroy()
    {
        Destroy(_zoomCard);
    }

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void HoverOver()
    {
        //gameObject.GetComponent<Outline>().enabled = true;
        //
        //Vector2 cardPosition = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), Canvas.GetComponent<Canvas>()).position;
        //cardPosition = GetComponent<RectTransform>().anchoredPosition;
        //
        //_zoomCard = Instantiate(gameObject, cardPosition, Quaternion.identity);
        //RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        //rect.anchorMax = new Vector2(0, 1);
        //rect.anchorMin = new Vector2(0, 1);
        //rect.pivot = new Vector2(0, 1);
        //
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
        gameObject.GetComponent<Outline>().enabled = true;

        Vector2 cardPosition = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), Canvas.GetComponent<Canvas>()).position;
        //cardPosition = GetComponent<RectTransform>().anchoredPosition;

        _zoomCard = Instantiate(gameObject, cardPosition, Quaternion.identity);
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Outline>().enabled = false;
        Destroy(_zoomCard);
    }
}