using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardName", menuName = "Card", order = 1)]
public class Card : ScriptableObject
{
    public string CardName;
    public int ManaCost;
    public int Attack;
    public int Health;
}
