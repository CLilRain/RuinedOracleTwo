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
        Enemy = FindAnyObjectByType<Enemy>().transform;
    }

    public void drawCardFromCardCollection()
    {
        if (isPlayerTurn)
        {
            // first check if player have more than 5 card , don't proceed

            spawnMainCard();
        }
    }

    private void spawnMainCard()
    {
        GameObject card = Instantiate(mainCardPrefab, transform.position, Quaternion.identity);

        Card _card = card.GetComponent<Card>();

        _card.cardData = getRandomCardData();
        _card.setCardToFlipMainSide();
    }


    public Cards_SO getRandomCardData()
    {
        int randomNumber = Random.Range(0, TotalCards.Count - 1);
        return TotalCards[randomNumber];
    }
}
