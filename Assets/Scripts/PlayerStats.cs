using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	public bool IsLocalPlayer;

	public bool IsFirstTurn = true;
	

	public int Health;
	public int Mana;
	public int MaxMana = 1;

	public List<GameObject> HandCards = new List<GameObject>();
	public List<GameObject> FieldCards = new List<GameObject>();

	public List<Card> Deck = new List<Card>();
}
