using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public Sprite DefaultCursor;
    public Sprite TargetCrosshair;

    public Image Renderer;
    public LayerMask TargetLayer; // card layer


    //public CardStats Card;

    private void Update()
    {
        gameObject.transform.position = Input.mousePosition;
    }

    public void SelectTarget(Vector2 mousePosition, bool isSelected, GameObject playerCard) // takes in the mouse position, if an attack card is selected and the selected card object
    {
        
        RaycastHit2D[] isHit = Physics2D.CircleCastAll(mousePosition, 0.1f, Vector2.zero, 1f, TargetLayer); // checks when a card collides with another thing

        if (isHit.Length > 0) // something has been hit
        {
            RaycastHit2D hit = isHit[0];

            CardStats card = hit.collider.gameObject.GetComponent<CardStats>(); // if its collided with anotger card get the stats

            bool isPlayerCard = card.BelongsToLocalPlayer; // get if the card is an opponent card or not // should be false to enable crosshair 
            int enemyZone = card.ZoneID; // get the ID of the zone the opponent card is in
            int playerZone = playerCard.GetComponent<CardStats>().ZoneID; // get the id of the player card zone

            // check if the player can target this card

            if (isSelected == true) // if the attack card is selected, if isplayercard is false, and the collision matches zones change cross hair
            {
                if(!isPlayerCard)
                {
                    if (playerZone == enemyZone)
                    {
                        Renderer.sprite = TargetCrosshair;
                    }
                    else
                    {
                        Renderer.sprite = DefaultCursor;
                    }
                }
                
            }
            else
            {
                Renderer.sprite = DefaultCursor;
            }


        }
        else if (Renderer.sprite != DefaultCursor)
        {
            Renderer.sprite = DefaultCursor;
        }

    }


}
