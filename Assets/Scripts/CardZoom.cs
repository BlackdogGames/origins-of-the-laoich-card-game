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
        gameObject.GetComponent<Outline>().enabled = true;
        _zoomCard = Instantiate(gameObject, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        _zoomCard.transform.SetParent(Canvas.transform, false);
        _zoomCard.layer = LayerMask.NameToLayer("Zoom"); // puts the new zoomed card to a seperate layer

        RectTransform rect = _zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(480, 708);
    }

    public void HoverExit()
    {
        gameObject.GetComponent<Outline>().enabled = false;
        Destroy(_zoomCard);
    }
}
