using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public List<Cards_SO> cardsInPlayerHand = new List<Cards_SO>();     // later make this { get ; private set;}


    // add list for all dead card 

    public bool cardDrawnAtPlayerTurn {  get; set; }

    private void Start()
    {
        StartCoroutine(spawnCardsInHand());
    }

    private void Update()
    {
        if(GameManager.instance.isPlayerTurn && !cardDrawnAtPlayerTurn)
        {
            cardDrawnAtPlayerTurn = true;
            GameManager.instance.drawCardFromCardCollection();
        }

    }

    public void setPlayerCards(List<Cards_SO> cardsWeWantForPlayer)     // use this method when we need to add card from somewhere else
    {
        for (int i = 0; i < cardsWeWantForPlayer.Count; i++)            // when the game start 
        {
            cardsInPlayerHand[i] = cardsWeWantForPlayer[i]; 
        }
    }

    private IEnumerator spawnCardsInHand()     // this is for when game start -- spawning card that player have selected
    {
        for (int i = 0; i < cardsInPlayerHand.Count; i++)
        {
            GameObject originalCard = 
                Instantiate(GameManager.instance.mainCardPrefab, Vector2.zero, transform.rotation);

            originalCard.GetComponent<CardMotion>().speed = 15;

            originalCard.GetComponent<Card>().cardData = cardsInPlayerHand[i];
            originalCard.GetComponent<Card>().setCardToFlipMainSide();

            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1.5f);

        GameManager.instance.turnUI.PlayPlayerTurnAnimation();      // when all card settles ,, play player turn animation
    }
}
