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
            //GameObject _playerCard = Instantiate(CardOne, new Vector3(0, 0, 0), Quaternion.identity); //  where a random card is instantiated from the list
            //_playerCard.transform.SetParent(PlayerArea.transform, false); // when object is instantiated, set it as child of PlayerArea
            //_playerStats.PlayerCards.Add(_playerCard);
            //
            //GameObject _enemyCard = Instantiate(CardOne, new Vector3(0, 0, 0), Quaternion.identity);
            //_enemyCard.transform.SetParent(OpponentArea.transform, false); // child of opponent area
            //_oppStats.OpponentCards.Add(_enemyCard);

            SceneGameManager.DrawCard(Player);
            SceneGameManager.DrawCard(Opponent);
        }

        
    }
    //
}
