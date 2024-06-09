using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public bool isPlayerTurn { get; set; }
    public bool isEnemyTurn { get; set; }


    public GameObject mainCardPrefab;
    public Transform Player {  get; private set; }
    public Transform Enemy { get; private set; }



    public Turn_UI turnUI { get; private set; }

    [SerializeField] private LineRenderer mainLine;
    public LineRenderer lineRenderer { get; private set; }


    [SerializeField] private List<Cards_SO> TotalCards = new List<Cards_SO>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }

        turnUI = FindAnyObjectByType<Turn_UI>();
        Player = FindAnyObjectByType<Player>().transform;
        Enemy = FindAnyObjectByType<EnemyAI>().transform;
    }

    private void Start()
    {
        lineRenderer = Instantiate(mainLine , Vector2.zero , Quaternion.identity);  // spawning LineRenderer for arrow 
    }

    public void drawCardFromCardCollection()
    {
        if (isPlayerTurn)
        {
            // first check if player have more than 5 card , don't proceed
            if(Player.GetComponent<Player>().cardsInPlayerHand.Count >= 5)
            {
                Debug.Log("No Space Left On Player Hand");
                return;
            }

            spawnMainCard(true);
        }
        else if(isEnemyTurn)
        {
            //if you want you can check limit for cards in enemy Hand
            //if cardsInEnemyHand > 5 -- not add more cards 
            spawnMainCard(false);
        }
    }

    private void spawnMainCard(bool _spawningForPlayer)
    {
        GameObject card = Instantiate(mainCardPrefab, transform.position, Quaternion.identity);

        Card _card = card.GetComponent<Card>();

        _card.cardData = getRandomCardData();
        
        if(_spawningForPlayer)
        {
            _card.setCardToFlipMainSide();
            Player.GetComponent<Player>().cardsInPlayerHand.Add(_card.cardData);
        }
        else
        {
            CardMotion _cardMotion = card.GetComponent<CardMotion>();

            _cardMotion.speed = 20;
            _cardMotion.isBelongToEnemy = true;
            _cardMotion.targetPoint = Enemy.GetComponent<EnemyAI>().enemyCardHoldPoint;

            Enemy.GetComponent<Enemy>().cardsInEnemyHand.Add(card);
        }
    }

    public Cards_SO getRandomCardData()
    {
        int randomNumber = Random.Range(0, TotalCards.Count - 1);
        return TotalCards[randomNumber];
    }
}
