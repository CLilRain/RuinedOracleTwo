using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public bool cardDrawLeft { get; set; } = true;


    private void Update()
    {
        if(GameManager.instance.isEnemyTurn && cardDrawLeft)
        {
            spawnCardForEnemy();
        }
    }
   
    public void spawnCardForEnemy()
    {
        cardDrawLeft = false;
        StartCoroutine(spawnCardForEnemyWithDelay(GameManager.instance.mainCardPrefab, GameManager.instance.getRandomCardData()));
    }

    private IEnumerator spawnCardForEnemyWithDelay(GameObject _mainCard, Cards_SO _cardData)
    {
        yield return new WaitForSeconds(2f);

        GameObject card =
            Instantiate(_mainCard, new Vector2(transform.position.x - 3, transform.position.y), Quaternion.identity);

        Card _card = card.GetComponent<Card>();
        CardMotion _cardMotion = card.GetComponent<CardMotion>();

        _card.cardData = _cardData;
        _card.setCardToFlipMainSide();

        _cardMotion.speed = 10;
        _cardMotion.isBelongToEnemy = true;
        _cardMotion.targetPoint = GameManager.instance.Player;
    }
}
