using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> TabButtons;
    public Sprite TabIdle;
    public Sprite TabHover;
    public Sprite TabActive;
    public TabButton SelectedTab;
    public List<GameObject> ObjectsToSwap;

    public void Subscribe(TabButton button) // TabGroup component, that tabs subscribe to.
    {
        if(TabButtons == null)
        {
            TabButtons = new List<TabButton>();
        }

        TabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button) // When mouse enters the image area, the background sprite state is updated
    {
        ResetTabs(); // Resets Sprite Icon
        if (SelectedTab == null || button != SelectedTab) // Only change the sprite if the tab isn't already selected
        {
            button.Background.sprite = TabHover; // Swap Sprite
        }
    }

    public void OnTabExit(TabButton button) // When mouse enters the image area, the background sprite state is updated
    {
        ResetTabs(); // Resets Sprite Icon
    }

    public void OnTabSelected(TabButton button) // When mouse enters the image area, the background sprite state is updated
    {
        if (SelectedTab != null)
        {
            SelectedTab.Deselect();
        }

        SelectedTab = button; 

        SelectedTab.Select();

        ResetTabs(); // Resets Sprite Icon
        button.Background.sprite = TabActive; // Swap Sprite
        int index = button.transform.GetSiblingIndex(); // Gets index of each tab image, each tab images index relates to each menu panel
        for (int i = 0; i < ObjectsToSwap.Count; i++) // Iterates through each object
        {
            if (i == index)
            {
                ObjectsToSwap[i].SetActive(true);
            }
            else
            {
                ObjectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() // Resets all Tabs to the "Idle" Sprite
    {
        foreach(TabButton button in TabButtons)
        {
            if (SelectedTab !=null && button == SelectedTab) { continue; } // Skips currently selected Tab
            button.Background.sprite = TabIdle; // Swap Sprite
        }
    }

}
