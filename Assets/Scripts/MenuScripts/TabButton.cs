using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler // UI Event States
{

    public TabGroup TabGroup;

    public Image Background;

    public UnityEvent OnTabSelected;
    public UnityEvent OnTabDeselected;

    public void Start()
    {
        Background = GetComponent<Image>();
        TabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData) // When mouse clicks UI elements, call OnTabSelected
    {
        TabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse enters UI element, call OnTabEnter
    {
        TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exits UI element, call OnTabExit
    {
        TabGroup.OnTabExit(this);
    }

    public void Select() // Callbacks, When tabs change state anything can be added/removed
    {
        if (OnTabSelected != null)
        {
            OnTabSelected.Invoke();
        }
    }

    public void Deselect() // Callbacks, When tabs change state anything can be added/removed
    {
        if (OnTabDeselected != null)
        {
            OnTabDeselected.Invoke();
        }
    }

}
