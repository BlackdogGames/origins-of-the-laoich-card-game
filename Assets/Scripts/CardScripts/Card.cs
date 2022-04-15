using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CardAbility : UnityEvent<GameManager, CardStats>
{
}

[CreateAssetMenu(fileName = "CardName", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public int ID;
    public string CardName;
    public int ManaCost;
    public int Attack;
    public int Health;
    public Sprite CardImage;

    public CardAbility Ability;

    Card()
    {
        Ability.AddListener(CardAbilities.EffieAbility);
    }
}
