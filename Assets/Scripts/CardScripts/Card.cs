using System;
using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CardAbility : UltEvent<GameManager, CardStats>
{
}

public delegate void CardDelegateAbility(GameManager gm, CardStats cs);

[CreateAssetMenu(fileName = "CardName", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{

    public int ID;
    public string CardName;
    public int ManaCost;
    public int Attack;
    public int Health;
    public Sprite CardImage;
    public string Description;
    public bool IsMonster;

    public enum CardAbilityTrigger
    {
        NoAbility = 0,
        OnPlayed = 1,
        OnDestroyed = 2,
        OnAttacking = 3,
        OnDefending = 4,
        OnTurnEnd = 5
    }

    public CardAbilityTrigger AbilityTrigger;
    public CardAbility Ability = new CardAbility();

    Card()
    {
        
    }
}
