using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    public Transform enemyCardHoldPoint;
    [SerializeField] private GameObject smallCard;

    public List<GameObject> smallCardsInFields = new List<GameObject>();


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
   
    public void spawnCardForEnemy()
    {
        enemy.cardDrawLeft = false;
        StartCoroutine(spawnCardForEnemyWithDelay());
    }

    private IEnumerator spawnCardForEnemyWithDelay()
    {
        yield return new WaitForSeconds(1f);

        GameManager.instance.drawCardFromCardCollection();  // on enemy turn - draw random card

        if(isSpaceLeftForEnemyCardInField(FindAnyObjectByType<Field>()))     //then check if you want that card on field
        {
            putCardOnBattleField();
        }
        else
        {
            //no space left in field for enemycard
            //switch turn or attack with any card -- for now i am using turn switch later make card attack forcefully

            // take any random small card from field (enemy card) -- in his smallcard script make attack player true

            int randomNumber = Random.Range(0, smallCardsInFields.Count);
            smallCardsInFields[randomNumber].GetComponent<SmallCard>().attackPlayer = true;

            //GameManager.instance.turnUI.PlayPlayerTurnAnimation();
        }
    }

    private void putCardOnBattleField()
    {
        StartCoroutine(putCardOnBattleFieldWithDelay());
    }

    private IEnumerator putCardOnBattleFieldWithDelay()
    {
        yield return new WaitForSeconds(2f);

        if (Random.Range(0, 51) * enemy.cardsInEnemyHand.Count > 25) // for testing -- using 0 - 25
        {
            Debug.Log("Go To Battle Field!");

            Field _field = FindAnyObjectByType<Field>();

            GameObject battleFieldCard = randomCardGoOnFieldFromHand();


            _field.setCardOnFieldForEnemy(battleFieldCard, true);

            //using 100 for test (0 , 100) -- and 0 also for testing
            bool _attack = Random.Range(0, 50) + 1000/GetComponent<EnemyHealth>().health > 40 ? true : false;    //40
            //bool _attack = 100 > 0 ? true : false; use this for testing

            enemy.cardsInEnemyHand.Remove(battleFieldCard);

            Card _card = battleFieldCard.GetComponent<Card>();

            settingUpSmallCardForEnemy(_card, battleFieldCard, _field, _attack);

            _card.setCardToFlipMainSide();

            battleFieldCard.SetActive(false);

            if(_attack)
            {
                _field.setCardOnFieldForEnemy(battleFieldCard, false);
            }
            else
            {
                if (smallCardsInFields.Count > 1)
                {
                    //check you have cards on field already ? run attack bool check again if it true then already exist card will attack 
                    bool attack = Random.Range(0, 50) + 1000 / GetComponent<EnemyHealth>().health > 30 / smallCardsInFields.Count ? true : false;

                    if (attack)
                    {
                        int randomCardNumber = 0;

                        if (Random.Range(1 , 101) > 50)    //50
                        {
                            Debug.Log("High Card");
                           randomCardNumber = smallCardsInFields.Count - 1;
                        }
                        else if(Random.Range(1 , 101) < 50) //50
                        {
                            Debug.Log("Low card");
                            randomCardNumber = Random.Range(0, smallCardsInFields.Count - 1);
                        }

                        smallCardsInFields[randomCardNumber].GetComponent<SmallCard>().attackPlayer = attack;
                    }
                }
            }
        }
        else
        {
            GameManager.instance.turnUI.PlayPlayerTurnAnimation();  // if enemy decided to not use card switch to player turn
        }
    }

    private bool isSpaceLeftForEnemyCardInField(Field _field)
    {
        if (_field.cardOnFieldForEnemy.Count >= 3)
        {
            Debug.LogWarning("No Space Left For Enemy");
            return false;
        }

        return true;
    }

    private GameObject randomCardGoOnFieldFromHand()
    {
        int randomCardValue = Random.Range(0, enemy.cardsInEnemyHand.Count);
        return enemy.cardsInEnemyHand[randomCardValue];
    }

    private void settingUpSmallCardForEnemy(Card _card , GameObject _battleFieldCard , Field _field , bool _attack)
    {
        GameObject _smallCard =
                Instantiate(smallCard, enemyCardHoldPoint.position, Quaternion.identity);

        _smallCard.GetComponent<SmallCard>().
            smallCardSetup(_card.cardData, _battleFieldCard, _card.cardData.smallIMG, enemyCardHoldPoint.position, _field.getCardPositionForEnemyInField().position, _field.getCardHoldPointIndexForEnemy(), true, _attack);

        smallCardsInFields.Add(_smallCard);
    }
}
