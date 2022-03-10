using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCards : MonoBehaviour
{
    //
    public GameManager SceneGameManager;

    public GameObject CardOne;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public GameObject Player;
    public GameObject Opponent;

    PlayerStats _playerStats;
    PlayerStats _oppStats;
    //

    //
    void Start()
    {
        SceneGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _playerStats = Player.GetComponent<PlayerStats>();
        _oppStats = Opponent.GetComponent<PlayerStats>();
    }
    //

    //
    public void OnClick()
    {
        for(int i = 0; i < 5; i++)
        {
            SceneGameManager.DrawCard(Player);
            SceneGameManager.DrawCard(Opponent);
        }
    }
    //
}
