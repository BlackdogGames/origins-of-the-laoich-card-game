using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    private GameObject _zoomCard;
    //

    //
    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public void HoverOver()
    {
        Vector2 cardPosition = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), Canvas.GetComponent<Canvas>()).position;
        cardPosition = GetComponent<RectTransform>().anchoredPosition;

        _zoomCard = Instantiate(gameObject, cardPosition, Quaternion.identity);
        RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        rect.anchorMax = new Vector2(0, 1);
        rect.anchorMin = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);

        _zoomCard.GetComponent<Image>().raycastTarget = false;
        _zoomCard.transform.SetParent(Canvas.transform, false);
        _zoomCard.layer = LayerMask.NameToLayer("Zoom"); // puts the new zoomed card to a seperate layer

        rect.sizeDelta = new Vector2(480, 708);
    }

    public void HoverExit()
    {
        gameObject.GetComponent<Outline>().enabled = false;
        Destroy(_zoomCard);
    }
}
