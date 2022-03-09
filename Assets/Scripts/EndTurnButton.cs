using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnClick()
    {
        if (_gameManager.PlayersTurn) // if it is the players turn that is ending and they have space for it, draw 1 new card
        {
            _gameManager.ManaIncrease(_gameManager.Player);

            if (_gameManager.PlayerStatsInstance.Cards.Count < 7)
            {
                _gameManager.DrawCard(_gameManager.Player);
            }
        } else // if it is the opponents turn that is ending and they have space for it, draw 1 new card
        {
            _gameManager.ManaIncrease(_gameManager.Opponent);
            if (_gameManager.OppStatsInstance.Cards.Count < 7)
            {
                _gameManager.DrawCard(_gameManager.Opponent);
            }
        }

        _gameManager.PlayersTurn = !_gameManager.PlayersTurn; // End turn
    }
}